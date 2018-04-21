Imports Newtonsoft.Json

Namespace Data.Warships

    Public Class PlayerShipStats
        Public Property status As String
        Public Property meta As Data.Common.Meta
        Public Property [Error] As Data.Common.Error
        Public Property Stats As New List(Of ShipStats)
        Public Property User As Common.User
    End Class

    Public Class ShipStats
        Public Property User As New Common.User
        Public Property distance As Integer = 0
        <JsonConverter(GetType(Converters.UnixDateTimeConverter))>
        Public Property last_battle_time As DateTime
        Public Property pvp As BattleStats
        Public Property pve As BattleStats
        Public Property club As BattleStats
        Public Property rank_solo As BattleStats
        Public Property oper_solo As Operation
        <JsonConverter(GetType(Converters.UnixDateTimeConverter))>
        Public Property updated_at As DateTime
        Public Property battles As Integer = 0
        Public ReadOnly Property statBattles As Integer
            Get
                Return pvp.battles + pve.battles + club.battles + rank_solo.battles + oper_solo.battles
            End Get
        End Property
        Public Property ship_id As Long
        Public Property _private As Object
    End Class

    Public Class BattleStats
        Public Property max_xp As Integer = 0
        Public Property damage_to_buildings As Integer = 0
        Public Property main_battery As Main_Battery
        Public Property suppressions_count As Integer = 0
        Public Property max_damage_scouting As Integer = 0
        Public Property art_agro As Integer = 0
        Public Property ships_spotted As Integer = 0
        Public Property second_battery As Second_Battery
        Public Property xp As Integer = 0
        Public Property survived_battles As Integer = 0
        Public Property dropped_capture_points As Integer = 0
        Public Property max_damage_dealt_to_buildings As Integer = 0
        Public Property torpedo_agro As Integer = 0
        Public Property draws As Integer = 0
        Public Property planes_killed As Integer = 0
        Public Property battles As Integer = 0
        Public Property max_ships_spotted As Integer = 0
        Public Property team_capture_points As Integer = 0
        Public Property frags As Integer = 0
        Public Property damage_scouting As Integer = 0
        Public Property max_total_agro As Integer = 0
        Public Property max_frags_battle As Integer = 0
        Public Property capture_points As Integer = 0
        Public Property ramming As Ramming
        Public Property torpedoes As Torpedoes
        Public Property aircraft As Aircraft
        Public Property survived_wins As Integer = 0
        Public Property max_damage_dealt As Integer = 0
        Public Property wins As Integer = 0
        Public Property losses As Integer = 0
        Public Property damage_dealt As Integer = 0
        Public Property max_planes_killed As Integer = 0
        Public Property max_suppressions_count As Integer = 0
        Public Property team_dropped_capture_points As Integer = 0
        Public Property battles_since_512 As Integer = 0
    End Class

    Public Class Main_Battery
        Public Property max_frags_battle As Integer = 0
        Public Property frags As Integer = 0
        Public Property hits As Integer = 0
        Public Property shots As Integer = 0
    End Class

    Public Class Second_Battery
        Public Property max_frags_battle As Integer = 0
        Public Property frags As Integer = 0
        Public Property hits As Integer = 0
        Public Property shots As Integer = 0
    End Class

    Public Class Ramming
        Public Property max_frags_battle As Integer = 0
        Public Property frags As Integer = 0
    End Class

    Public Class Torpedoes
        Public Property max_frags_battle As Integer = 0
        Public Property frags As Integer = 0
        Public Property hits As Integer = 0
        Public Property shots As Integer = 0
    End Class

    Public Class Aircraft
        Public Property max_frags_battle As Integer = 0
        Public Property frags As Integer = 0
    End Class


    Public Class Operation
        Public Property wins As Integer = 0
        Public Property losses As Integer = 0
        Public Property battles As Integer = 0
        Public Property survived_wins As Integer = 0
        Public Property xp As Integer = 0
        Public Property wins_by_tasks As Object
        Public Property survived_battles As Integer = 0
    End Class

End Namespace



