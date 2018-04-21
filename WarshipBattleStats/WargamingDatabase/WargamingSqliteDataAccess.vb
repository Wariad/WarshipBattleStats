

Public Delegate Sub DataProgressCallbackDelegate(msg As String, image As Drawing.Image, total As Integer, progress As Integer)

Public Class WargamingSqliteDataAccess
    Inherits DataAccess

#Region "Declares..."

#If DEBUG Then
    Private Shared _ConnectionString As String = String.Format(My.Settings.SqliteConnectionString, My.Computer.FileSystem.SpecialDirectories.MyDocuments.Replace("\", "/"))
#Else
    Private Shared _ConnectionString As String = String.Format(My.Settings.SqliteConnectionString, My.Application.Info.DirectoryPath.Replace("\", "/"))
#End If


#End Region

#Region "New..."

#End Region

#Region "Methods..."

    Public Shared Function GetUserAccounts() As List(Of Data.Common.User)

        Dim Users As New List(Of Data.Common.User)

        Using Db As New SQLite.SQLiteConnection(_ConnectionString)
            Db.Open()
            Using cmd As New SQLite.SQLiteCommand(Queryies.GetUserAccounts, Db)
                Using reader As IDataReader = cmd.ExecuteReader()
                    While reader.Read
                        Dim user As New Data.Common.User
                        user.Nickname = GetString(reader, 1)
                        user.Account = GetString(reader, 2)
                        user.Server = GetEnum(Of Data.Common.ServerRealm)(reader, 3)
                        Users.Add(user)
                    End While

                End Using

            End Using

            Db.Close()

        End Using

        Return Users

    End Function

    Public Shared Sub DeleteUserAccounts(accounts As List(Of Data.Common.User))

        For Each acc As Data.Common.User In accounts
            ExecuteNonQuery(Queryies.DeleteUserAccount(acc))
        Next

    End Sub

    Public Shared Sub AddUserAccounts(accounts As List(Of Data.Common.User))

        For Each acc As Data.Common.User In accounts
            If Not ExecuteScalar(Of Boolean)(Queryies.UserAccountExists(acc)) Then
                ExecuteNonQuery(Queryies.AddUserAccount(acc))
            End If
        Next

    End Sub

    Public Shared Sub AddShipStats(stats As List(Of Data.Warships.ShipStats), Optional callback As DataProgressCallbackDelegate = Nothing)

        Dim record As Integer = 1
        For Each Stat As Data.Warships.ShipStats In stats

            If callback IsNot Nothing Then
                Dim msg As String = String.Format("{0} / {1}", Stat.User.ToShortString, ShipNameFromID(Stat.ship_id))
                callback(msg, ShipImageFromID(Stat.ship_id), stats.Count, record)
            End If

            If Not ExecuteScalar(Of Boolean)(Queryies.StatsShipExists(Stat)) Then

                ExecuteNonQuery(Queryies.AddShip(Stat))
                Dim stats_ID As Long = ExecuteScalar(Of Long)(Queryies.GetLastShipInsertID(Stat))

                If stats_ID <> 0 Then
                    AddBattleStats(Stat.ship_id, "PVP", stats_ID, Stat.pvp)
                    AddBattleStats(Stat.ship_id, "PVE", stats_ID, Stat.pve)
                    AddBattleStats(Stat.ship_id, "CLAN", stats_ID, Stat.club)
                    AddBattleStats(Stat.ship_id, "RANK", stats_ID, Stat.rank_solo)
                End If

            End If
            record += 1
        Next

    End Sub

    Private Shared Sub AddOpperationStats(stats_ID As Long, stats As Data.Warships.Operation)

        ' Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        'Db.ExecuteScalar("dbo.stats_AddOpperationStats", stats_ID,
        '                                                stats.battles,
        '                                                stats.wins,
        '                                                stats.losses,
        '                                                stats.xp,
        '                                                stats.survived_battles,
        '                                                stats.survived_wins)

    End Sub

    Private Shared Sub AddBattleStats(shipID As Long, statType As String, updateID As Long, stats As Data.Warships.BattleStats)

        If stats.battles = 0 Then Return

        If ExecuteScalar(Of Boolean)(Queryies.ShipStatExists(shipID, updateID, stats.battles, statType)) Then
            Return
        End If

        ExecuteNonQuery(Queryies.AddBattleStats(statType, updateID, stats))
        Dim statID As Long = ExecuteScalar(Of Long)(Queryies.GetLastStatInsertID(updateID, statType))

        ExecuteNonQuery(Queryies.AddMainBattery(statID, stats.main_battery))
        ExecuteNonQuery(Queryies.AddSecondaryBattery(statID, stats.second_battery))
        ExecuteNonQuery(Queryies.AddTorpedoes(statID, stats.torpedoes))
        ExecuteNonQuery(Queryies.AddRamming(statID, stats.ramming))
        ExecuteNonQuery(Queryies.AddAircraft(statID, stats.aircraft))

    End Sub

    Public Shared Sub AddAchievments(achievements As List(Of Data.Encyclopedia.Achievment))

        'Dim converter As New ImageConverter
        'Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        'For Each achievement As Data.Encyclopedia.Achievment In achievements

        '    Dim battleTypes As New StringBuilder
        '    For Each s As String In achievement.battle_types
        '        battleTypes.Append(s & ":")
        '    Next

        '    If battleTypes.Length <> 0 Then
        '        battleTypes.Replace(battleTypes.ToString, battleTypes.ToString.Substring(0, battleTypes.Length - 1))
        '    End If

        '    Db.ExecuteNonQuery("dbo.Encyc_AddAchievment", achievement.name,
        '                                            battleTypes.ToString,
        '                                            achievement.multiple,
        '                                            achievement.count_per_battle,
        '                                            achievement.max_progress,
        '                                            achievement.is_progress,
        '                                            converter.ConvertTo(achievement.activeImage, GetType(Byte())),
        '                                            converter.ConvertTo(achievement.inactiveImage, GetType(Byte())),
        '                                            achievement.max_ship_level,
        '                                            achievement.min_ship_level,
        '                                            achievement.achievement_id,
        '                                            achievement.hidden,
        '                                            achievement.reward,
        '                                            achievement.type,
        '                                            achievement.sub_type,
        '                                            achievement.description)

        'Next

    End Sub

    Public Shared Sub AddUserAchievments(accountID As String, achievements As List(Of Data.Account.AchievementData))

        'Dim converter As New ImageConverter
        'Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        'For Each achievement As Data.Account.AchievementData In achievements

        '    Db.ExecuteNonQuery("dbo.stats_AddUserAchievement", accountID,
        '                                                        DateTime.Now.Date,
        '                                                        achievement.AchievmentID,
        '                                                        achievement.Count
        '                                                        )

        'Next

    End Sub

    Public Shared Sub AddEncyclopediaShips(ships As List(Of Data.Encyclopedia.Ship))

        Dim converter As New Drawing.ImageConverter

        For Each ship As Data.Encyclopedia.Ship In ships

            If ExecuteScalar(Of Boolean)(Queryies.EncyclopediaShipExists(ship.ship_id)) Then
                ExecuteNonQuery(Queryies.UpdateEncyclopediaShip(ship))
            Else
                ExecuteNonQuery(Queryies.AddEncyclopediaShip(ship))
            End If

        Next

    End Sub

    Public Shared Function GetBattleLog(account As Data.Common.User, numItems As Integer) As Data.Stats.BattleLog

        Dim Log As New Data.Stats.BattleLog

        Using Db As New SQLite.SQLiteConnection(_ConnectionString)
            Db.Open()
            Using cmd As New SQLite.SQLiteCommand(Queryies.GetBattleLog(account, numItems), Db)
                Using reader As IDataReader = cmd.ExecuteReader()
                    While reader.Read
                        Dim item As New Data.Stats.BattleLogItem
                        item.Ship.ShipID = GetInt64(reader, 3)
                        item.Ship.ShipIDString = GetString(reader, 4)
                        item.Ship.LastBattleTime = GetSqlDateTime(reader, 5)
                        item.Ship.Name = GetString(reader, 6)
                        item.Ship.Nation = GetString(reader, 7)
                        item.Ship.Type = GetString(reader, 8)
                        item.Ship.Tier.Value = GetByte(reader, 9)
                        item.Ship.BattleType = GetString(reader, 10)
                        item.Battles = GetInt32(reader, 11)
                        item.Win = GetBool(reader, 12)
                        item.Draw = GetBool(reader, 13)
                        item.Survived = GetBool(reader, 14)
                        item.Frags = GetInt32(reader, 15)
                        item.PlanesKilled = GetInt32(reader, 16)
                        item.DamageDelt = GetInt32(reader, 17)
                        item.CapturePoints = GetInt32(reader, 18)
                        item.TopedoShots = GetInt32(reader, 19)
                        item.TorpedoHits = GetInt32(reader, 20)
                        item.TorpedoFrags = GetInt32(reader, 21)
                        item.MainBatteryShots = GetInt32(reader, 22)
                        item.MainBatteryHits = GetInt32(reader, 23)
                        item.MainBatteryFrags = GetInt32(reader, 24)
                        item.Ship.Image = GetImage(reader, 25)
                        item.WR = GetDecimal(reader, 26)
                        item.FR = GetDecimal(reader, 27)
                        item.SR = GetDecimal(reader, 28)
                        item.DR = GetDecimal(reader, 29)
                        item.DMG = GetDecimal(reader, 30)

                        Log.Add(item)

                    End While
                End Using

            End Using
            Db.Close()
        End Using

        Return Log

    End Function

    Private Shared Function ShipNameFromID(id As Long) As String

        Return ExecuteScalar(Of String)(Queryies.ShipNameFromID(id))

    End Function

    Private Shared Function ShipImageFromID(id As Long) As Drawing.Image

        Dim image As Drawing.Image = Nothing

        Using Db As New SQLite.SQLiteConnection(_ConnectionString)
            Db.Open()
            Using cmd As New SQLite.SQLiteCommand(Queryies.ShipImageFromID(id), Db)
                Using reader As IDataReader = cmd.ExecuteReader()
                    reader.Read()
                    image = GetImage(reader, 0)
                End Using
            End Using
            Db.Close()
        End Using

        Return image

    End Function

    Private Shared Sub ExecuteNonQuery(query As String)

        Using cmd As New SQLite.SQLiteCommand(query)
            ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Shared Sub ExecuteNonQuery(cmd As SQLite.SQLiteCommand)

        Using Db As New SQLite.SQLiteConnection(_ConnectionString)
            Db.Open()
            cmd.Connection = Db
            cmd.ExecuteNonQuery()
            Db.Close()
        End Using

    End Sub

    Private Shared Function ExecuteScalar(Of T)(query As String) As T

        Using Db As New SQLite.SQLiteConnection(_ConnectionString)
            Db.Open()
            Using cmd As New SQLite.SQLiteCommand(query, Db)
                Return CType(cmd.ExecuteScalar(), T)
            End Using
            Db.Close()
        End Using

    End Function

    Private Shared Function GetInsertID() As Long

        Return ExecuteScalar(Of Long)("select last_insert_rowid()")

    End Function

#End Region

#Region "Properties..."

#End Region

    Private Class Queryies

        Public Shared Function ShipImageFromID(id As Long) As String

            Return String.Format("select Smallimage from encyc_Ships where ShipID = {0}", id)

        End Function

        Public Shared Function ShipNameFromID(id As Long) As String

            Return String.Format("select Name from encyc_Ships where ShipID = {0}", id)

        End Function

        Public Shared Function DeleteUserAccount(user As Data.Common.User) As String

            Return String.Format("delete from stats_UserAccounts where AccountNumber = '{0}' and server = {1}", user.Account, [Enum].GetName(GetType(Data.Common.ServerRealm), user.Server))

        End Function

        Public Shared Function GetUserAccounts() As String

            Return "select * from stats_UserAccounts"

        End Function

        Public Shared Function UserAccountExists(user As Data.Common.User) As String

            Return String.Format("select    count(*)
                                from        stats_UserAccounts
                                where       server = '{0}'
                                and         Nickname = '{1}'",
                                user.ServerToString,
                                 user.Nickname)

        End Function

        Public Shared Function AddUserAccount(user As Data.Common.User) As String

            Return String.Format("insert into stats_UserAccounts (
                                                                 Server,
                                                                 Nickname,
                                                                 AccountNumber
                                                                )
                                                values          (
                                                                '{0}',
                                                                '{1}',
                                                                '{2}'
                                                                )",
                                                                user.ServerToString,
                                                                user.Nickname,
                                                                user.Account)

        End Function

        Public Shared Function StatsShipExists(ship As Data.Warships.ShipStats) As String

            Return String.Format("Select            count(*) 
                                    From            stats_Ships s 
                                    inner Join     stats_UserAccounts acc on s.UserID = acc.RecordID
                                    where             s.ShipID = {0}
                                    And             s.Battles = {1}
                                    And             acc.Server = '{2}'
                                    And            acc.AccountNumber = '{3}'",
                                     ship.ship_id,
                                     ship.battles,
                                     ship.User.ServerToString,
                                     ship.User.Account
                                     )


        End Function

        Public Shared Function ShipStatExists(shipID As Long, stats_ID As Long, battles As Integer, statType As String) As String

            Return String.Format("SELECT  count(*)
                                          FROM stats_Ships s
                                          INNER JOIN stats_BattleStats bs ON s.RecordID = bs.UpdateID
                                          WHERE s.shipid = {0} 
                                          AND bs.Battles = {1}  
                                          AND bs.BattleType = '{2}' 
                                          AND s.UserID = (select UserID from stats_Ships s2 where s2.RecordID = {3})",
                                            shipID,
                                             battles,
                                             statType,
                                             stats_ID
                                             )

        End Function

        Public Shared Function GetBattleLog(account As Data.Common.User, numItems As Integer) As String

            Return String.Format("select * from view_BattleLog where Server = '{0}' and AccountNumber = '{1}' LIMIT {2}", account.ServerToString, account.Account, numItems)

        End Function

        Public Shared Function AddShip(ship As Data.Warships.ShipStats) As String


            Return String.Format("Insert into stats_Ships	(
                                                            UserID,
                                                            LastBattleTime,
                                                            Distance,
                                                            Battles,
                                                            UpdatedAt,
                                                            ShipID  
								                            )
			                            values              (
								                            (select RecordID from stats_UserAccounts where Server = '{0}' and accountNumber = '{1}' group by RecordID),
								                            '{2}',
								                            {3},
								                            {4},
								                            '{5}',
                                                            {6}
                                                            )",
                                                            ship.User.ServerToString,
                                                            ship.User.Account,
                                                            DateTimeToSlqDateTime(ship.last_battle_time),
                                                            ship.distance,
                                                            ship.battles,
                                                            DateTimeToSlqDateTime(ship.updated_at),
                                                            ship.ship_id
                                                            )

        End Function

        Public Shared Function GetLastShipInsertID(ship As Data.Warships.ShipStats) As String

            Return String.Format("Select            s.RecordID 
                                    From            stats_Ships s 
                                    inner Join     stats_UserAccounts acc on s.UserID = acc.RecordID
                                    where             s.ShipID = {0}
                                    And             s.Battles = {1}
                                    And             acc.Server = '{2}'
                                    And            acc.AccountNumber = '{3}'",
                                     ship.ship_id,
                                     ship.battles,
                                     ship.User.ServerToString,
                                     ship.User.Account
                                     )

        End Function

        Public Shared Function GetLastStatInsertID(statID As Long, statType As String) As String

            Return String.Format("select RecordID from stats_BattleStats where UpdateID = {0} and BattleType = '{1}'", statID, statType)

        End Function

        Public Shared Function AddBattleStats(battleType As String, stats_ID As Long, stats As Data.Warships.BattleStats) As String

            Return String.Format("insert into stats_BattleStats (	
                                                         updateID,
							                             BattleType,
							                             maxXP,
							                             damageToBuildings,
							                             suppressionsCount,
							                             maxDamageScouting,
							                             artAgro,
							                             shipsSpotted,
							                             xP,
							                             survivedBattles,
							                             droppedCapturePoints,
							                             maxDamageToBuildings,
							                             torpedoAgro,
							                             draws,
							                             planesKilled,
							                             battles,
							                             maxShipsSpotted,
							                             teamCapturePoints,
							                             frags,
							                             damageScouting,
							                             maxTotalAgro,
							                             maxFragsBattle,
							                             capturePoints,
							                             survivedWins, 
							                             maxDamageDelt,
							                             wins,
							                             losses,
							                             damageDelt,
							                             maxPlanesKilled,
							                             maxSuppresionsCount,
							                             teamDroppedCapturePoints,
							                             battlesSince512
                                                         )
                                              values     (
                                                        {0},'{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},
                                                        {21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31}
                                                         )",
                                                       stats_ID,
                                                       battleType,
                                                       stats.max_xp,
                                                       stats.damage_to_buildings,
                                                        stats.suppressions_count,
                                                       stats.max_damage_scouting,
                                                       stats.art_agro,
                                                       stats.ships_spotted,
                                                       stats.xp,
                                                       stats.survived_battles,
                                                       stats.dropped_capture_points,
                                                       stats.max_damage_dealt_to_buildings,
                                                       stats.torpedo_agro,
                                                       stats.draws,
                                                       stats.planes_killed,
                                                       stats.battles,
                                                       stats.max_ships_spotted,
                                                       stats.team_capture_points,
                                                       stats.frags,
                                                       stats.damage_scouting,
                                                       stats.max_total_agro,
                                                       stats.max_frags_battle,
                                                       stats.capture_points,
                                                       stats.survived_wins,
                                                       stats.max_damage_dealt,
                                                       stats.wins,
                                                       stats.losses,
                                                       stats.damage_dealt,
                                                       stats.max_planes_killed,
                                                       stats.max_suppressions_count,
                                                       stats.team_dropped_capture_points,
                                                       stats.battles_since_512
                                                       )

        End Function

        Public Shared Function AddMainBattery(statID As Long, data As Data.Warships.Main_Battery) As String

            Return String.Format("insert into stats_MainBattery (
                                                                 BattleStatsID,
                                                                 shots,
                                                                 hits,
                                                                 frags,
                                                                 maxfrags
                                                		         )  
                                                       values    (
            			                                         {0},
            			                                         {1},
                                                                 {2},
                                                                 {3},
                                                                 {4}
                                                                 )",
                                                                 statID,
                                                                 data.shots,
                                                                 data.hits,
                                                                 data.frags,
                                                                 data.max_frags_battle
                                                                 )

        End Function

        Public Shared Function AddSecondaryBattery(statID As Long, data As Data.Warships.Second_Battery) As String

            Return String.Format("insert into stats_SecondaryBattery (
                                                                     BattleStatsID,
                                                                     shots,
                                                                     hits,
                                                                     frags,
                                                                     maxfrags
                                                		             )  
                                                           values    (
            			                                             {0},
            			                                             {1},
                                                                     {2},
                                                                     {3},
                                                                     {4}
                                                                     )",
                                                                     statID,
                                                                     data.shots,
                                                                     data.hits,
                                                                     data.frags,
                                                                     data.max_frags_battle
                                                                     )

        End Function

        Public Shared Function AddTorpedoes(statID As Long, data As Data.Warships.Torpedoes) As String

            Return String.Format("insert into stats_Torpedoes (
                                                                 BattleStatsID,
                                                                 shots,
                                                                 hits,
                                                                 frags,
                                                                 maxfrags
                                                		         )  
                                                       values    (
            			                                         {0},
            			                                         {1},
                                                                 {2},
                                                                 {3},
                                                                 {4}
                                                                 )",
                                                                 statID,
                                                                 data.shots,
                                                                 data.hits,
                                                                 data.frags,
                                                                 data.max_frags_battle
                                                                 )

        End Function

        Public Shared Function AddRamming(statID As Long, data As Data.Warships.Ramming) As String

            Return String.Format("insert into stats_Ramming (
                                                                 BattleStatsID,
                                                                 frags,
                                                                 maxfrags
                                                		         )  
                                                       values    (
            			                                         {0},
            			                                         {1},
                                                                 {2}
                                                                 )",
                                                                 statID,
                                                                 data.frags,
                                                                 data.max_frags_battle
                                                                 )

        End Function

        Public Shared Function AddAircraft(statID As Long, data As Data.Warships.Aircraft) As String

            Return String.Format("insert into stats_Aircraft(
                                                            BattleStatsID,
                                                            frags,
                                                            maxfrags
                                                		    )  
                                                values     (
            			                                    {0},
            			                                    {1},
                                                            {2}
                                                            )",
                                                            statID,
                                                            data.frags,
                                                            data.max_frags_battle
                                                            )

        End Function

        Public Shared Function EncyclopediaShipExists(shipID As Long) As String

            Return String.Format("select count(*) from encyc_ships where ShipID = {0}", shipID)

        End Function

        Public Shared Function AddEncyclopediaShip(ship As Data.Encyclopedia.Ship) As SQLite.SQLiteCommand

            Dim Converter As New Drawing.ImageConverter

            Dim query As String = String.Format("insert into encyc_Ships (
                                                          ShipID,
                                                          ShipIDString,
                                                          Description,
                                                          PriceGold,
                                                          Name,
                                                          Nation,
                                                          IsPremium,
                                                          PriceCredit,
                                                          Type,
                                                          Smallimage,
                                                          ShipTier
									                      )						
						                          values  (
                                                          {0},
                                                          '{1}',
                                                          '{2}',
                                                          {3},
                                                          '{4}',
                                                          '{5}',
                                                          {6},
                                                          {7},
                                                          '{8}',
                                                          @image,
                                                          {9}
                                                           )",
                                                          ship.ship_id,
                                                          ship.ship_id_str,
                                                          ship.description,
                                                          ship.price_gold,
                                                          ship.name,
                                                          ship.nation,
                                                          CInt(ship.is_premium),
                                                          ship.price_credit,
                                                          ship.type,
                                                          ship.tier)



            Dim cmd As New SQLite.SQLiteCommand()
            cmd.CommandText = query

            Dim pram As New SQLite.SQLiteParameter()
            pram.ParameterName = "@image"
            pram.Value = Converter.ConvertTo(ship.images.smallImage, GetType(Byte()))

            cmd.Parameters.Add(pram)

            Return cmd

        End Function

        Public Shared Function UpdateEncyclopediaShip(ship As Data.Encyclopedia.Ship) As SQLite.SQLiteCommand

            Dim Converter As New Drawing.ImageConverter

            Dim query As String = String.Format("update encyc_Ships Set	ShipIDString = '{0}',
                                                            Description = '{1}',
									                        PriceGold = {2},
									                        Name = '{3}',		
									                        Nation = '{4}',		
									                        IsPremium = {5},	
									                        PriceCredit = {6},
									                        Type = '{7}',			
									                        Smallimage = @image,		
									                        ShipTier = {8}
						                        where       ShipID = {9}",
                                                            ship.ship_id_str,
                                                            StringToSqlString(ship.description),
                                                            ship.price_gold,
                                                            StringToSqlString(ship.name),
                                                            ship.nation,
                                                            CInt(ship.is_premium),
                                                            ship.price_credit,
                                                            ship.type,
                                                            ship.tier,
                                                            ship.ship_id)


            Dim cmd As New SQLite.SQLiteCommand()
            cmd.CommandText = query

            Dim pram As New SQLite.SQLiteParameter()
            pram.ParameterName = "@image"
            pram.Value = Converter.ConvertTo(ship.images.smallImage, GetType(Byte()))

            cmd.Parameters.Add(pram)

            Return cmd

        End Function

    End Class

End Class
