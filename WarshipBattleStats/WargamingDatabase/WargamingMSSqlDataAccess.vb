
Imports WargamingDatabase
Imports System.Text
Imports System.Drawing
Imports Entlib = Microsoft.Practices.EnterpriseLibrary.Data

Public Class WargamingMSSqlDataAccess
    Inherits DataAccess

#Region "Declares..."

    Private Shared _ConnectionString As String = "Wargaming_FILE"

#End Region

#Region "New..."

#End Region

#Region "Methods..."

    Public Shared Sub AddUser(ByVal user As Data.Account.FoundPlayer)

        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        Db.ExecuteNonQuery("dbo.AddUser", user.account_id,
                                          user.nickname)

    End Sub

    Public Shared Sub AddShipStats(stats As List(Of Data.Warships.ShipStats))

        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        For Each Stat As Data.Warships.ShipStats In stats

            Dim stats_ID As Long = CLng(Db.ExecuteScalar("dbo.stats_AddShip", Stat.account.Account,
                                                    Stat.last_battle_time,
                                                    Stat.distance,
                                                    Stat.statBattles,
                                                    Stat.updated_at,
                                                    Stat.ship_id))

            If stats_ID <> 0 Then
                AddBattleStats("PVP", stats_ID, Stat.pvp)
                AddBattleStats("PVE", stats_ID, Stat.pve)
                AddBattleStats("CLAN", stats_ID, Stat.club)
                AddBattleStats("RANK", stats_ID, Stat.rank_solo)
            End If

        Next

    End Sub

    Private Shared Sub AddOpperationStats(stats_ID As Long, stats As Data.Warships.Operation)

        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        Db.ExecuteScalar("dbo.stats_AddOpperationStats", stats_ID,
                                                        stats.battles,
                                                        stats.wins,
                                                        stats.losses,
                                                        stats.xp,
                                                        stats.survived_battles,
                                                        stats.survived_wins)

    End Sub

    Private Shared Sub AddBattleStats(statType As String, stats_ID As Long, stats As Data.Warships.BattleStats)

        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        Db.ExecuteScalar("dbo.stats_AddBattleStats", stats_ID,
                                                statType,
                                                stats.max_xp,
                                                stats.damage_to_buildings,
                                                stats.main_battery.shots,
                                                stats.main_battery.hits,
                                                stats.main_battery.frags,
                                                stats.main_battery.max_frags_battle,
                                                stats.suppressions_count,
                                                stats.max_damage_scouting,
                                                stats.art_agro,
                                                stats.ships_spotted,
                                                stats.second_battery.shots,
                                                stats.second_battery.hits,
                                                stats.second_battery.frags,
                                                stats.second_battery.max_frags_battle,
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
                                                stats.ramming.frags,
                                                stats.ramming.max_frags_battle,
                                                stats.torpedoes.shots,
                                                stats.torpedoes.hits,
                                                stats.torpedoes.frags,
                                                stats.torpedoes.max_frags_battle,
                                                stats.aircraft.frags,
                                                stats.aircraft.max_frags_battle,
                                                stats.survived_wins,
                                                stats.max_damage_dealt,
                                                stats.wins,
                                                stats.losses,
                                                stats.damage_dealt,
                                                stats.max_planes_killed,
                                                stats.max_suppressions_count,
                                                stats.team_dropped_capture_points,
                                                stats.battles_since_512)

    End Sub

    Public Shared Sub AddAchievments(achievements As List(Of Data.Encyclopedia.Achievment))

        Dim converter As New ImageConverter
        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        For Each achievement As Data.Encyclopedia.Achievment In achievements

            Dim battleTypes As New StringBuilder
            For Each s As String In achievement.battle_types
                battleTypes.Append(s & ":")
            Next

            If battleTypes.Length <> 0 Then
                battleTypes.Replace(battleTypes.ToString, battleTypes.ToString.Substring(0, battleTypes.Length - 1))
            End If

            Db.ExecuteNonQuery("dbo.Encyc_AddAchievment", achievement.name,
                                                    battleTypes.ToString,
                                                    achievement.multiple,
                                                    achievement.count_per_battle,
                                                    achievement.max_progress,
                                                    achievement.is_progress,
                                                    converter.ConvertTo(achievement.activeImage, GetType(Byte())),
                                                    converter.ConvertTo(achievement.inactiveImage, GetType(Byte())),
                                                    achievement.max_ship_level,
                                                    achievement.min_ship_level,
                                                    achievement.achievement_id,
                                                    achievement.hidden,
                                                    achievement.reward,
                                                    achievement.type,
                                                    achievement.sub_type,
                                                    achievement.description)

        Next

    End Sub

    Public Shared Sub AddUserAchievments(accountID As String, achievements As List(Of Data.Account.AchievementData))

        Dim converter As New ImageConverter
        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        For Each achievement As Data.Account.AchievementData In achievements

            Db.ExecuteNonQuery("dbo.stats_AddUserAchievement", accountID,
                                                                DateTime.Now.Date,
                                                                achievement.AchievmentID,
                                                                achievement.Count
                                                                )

        Next

    End Sub

    Public Shared Sub AddEncyclopediaShips(ships As List(Of Data.Encyclopedia.Ship))

        Dim converter As New ImageConverter
        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)

        For Each ship As Data.Encyclopedia.Ship In ships

            Db.ExecuteNonQuery("dbo.encyc_AddShip", ship.ship_id,
                                                    ship.ship_id_str,
                                                    ship.description,
                                                    ship.price_gold,
                                                    ship.name,
                                                    ship.nation,
                                                    ship.is_premium,
                                                    ship.price_credit,
                                                    ship.type,
                                                    converter.ConvertTo(ship.images.smallImage, GetType(Byte())),
                                                    converter.ConvertTo(ship.images.mediumImage, GetType(Byte())),
                                                    converter.ConvertTo(ship.images.largeImage, GetType(Byte())),
                                                    converter.ConvertTo(ship.images.contourImage, GetType(Byte())),
                                                    ship.tier)


        Next

    End Sub

    Public Shared Function GetBattleLog(numItems As Integer) As Data.Stats.BattleLog

        Dim converter As New ImageConverter
        Dim Db As Entlib.Database = CreateDatabase(_ConnectionString)


        Dim Log As New Data.Stats.BattleLog
        Using reader As IDataReader = Db.ExecuteReader("dbo.stats_GetBattleLog", numItems)
            While reader.Read
                Dim item As New Data.Stats.BattleLogItem
                item.Ship.ShipID = GetInt64(reader, 0)
                item.Ship.ShipIDString = GetString(reader, 1)
                item.Ship.LastBattleTime = GetDate(reader, 2)
                item.Ship.Name = GetString(reader, 3)
                item.Ship.Nation = GetString(reader, 4)
                item.Ship.Type = GetString(reader, 5)
                item.Ship.Tier.Value = GetByte(reader, 6)
                item.Ship.BattleType = GetString(reader, 7)
                item.Battles = GetInt32(reader, 8)
                item.Win = GetBool(reader, 9)
                item.Draw = GetBool(reader, 10)
                item.Survived = GetBool(reader, 11)
                item.Frags = GetInt32(reader, 12)
                item.PlanesKilled = GetInt32(reader, 13)
                item.DamageDelt = GetInt32(reader, 14)
                item.CapturePoints = GetInt32(reader, 15)
                item.TopedoShots = GetInt32(reader, 16)
                item.TorpedoHits = GetInt32(reader, 17)
                item.TorpedoFrags = GetInt32(reader, 18)
                item.MainBatteryShots = GetInt32(reader, 19)
                item.MainBatteryHits = GetInt32(reader, 20)
                item.MainBatteryFrags = GetInt32(reader, 21)
                item.AircraftFrags = GetInt32(reader, 22)
                item.Ship.Image = GetImage(reader, 23)
                item.WR = GetDecimal(reader, 24)
                item.FR = GetDecimal(reader, 25)
                item.SR = GetDecimal(reader, 26)
                item.DR = GetDecimal(reader, 27)
                item.DMG = GetDecimal(reader, 28)



                Log.Add(item)

            End While
        End Using

        Return Log

    End Function

#End Region

#Region "Properties..."

#End Region



End Class
