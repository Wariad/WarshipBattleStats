
Namespace Data.Warships

    Public Class PlayerShipStats
        Public Property status As String
        Public Property meta As Data.Common.Meta
        Public Property [Error] As Data.Common.Error
        Public Property Stats As List(Of ShipStats)
    End Class

    Public Class ShipStats
        Public Property distance As Integer
        Public Property last_battle_time As Integer
        Public Property account_id As Integer
        Public Property pvp As Pvp
        Public Property updated_at As Integer
        Public Property battles As Integer
        Public Property ship_id As Long
        Public Property _private As Object
    End Class

    Public Class Pvp
        Public Property max_xp As Integer
        Public Property damage_to_buildings As Integer
        Public Property main_battery As Main_Battery
        Public Property suppressions_count As Integer
        Public Property max_damage_scouting As Integer
        Public Property art_agro As Integer
        Public Property ships_spotted As Integer
        Public Property second_battery As Second_Battery
        Public Property xp As Integer
        Public Property survived_battles As Integer
        Public Property dropped_capture_points As Integer
        Public Property max_damage_dealt_to_buildings As Integer
        Public Property torpedo_agro As Integer
        Public Property draws As Integer
        Public Property planes_killed As Integer
        Public Property battles As Integer
        Public Property max_ships_spotted As Integer
        Public Property team_capture_points As Integer
        Public Property frags As Integer
        Public Property damage_scouting As Integer
        Public Property max_total_agro As Integer
        Public Property max_frags_battle As Integer
        Public Property capture_points As Integer
        Public Property ramming As Ramming
        Public Property torpedoes As Torpedoes
        Public Property aircraft As Aircraft
        Public Property survived_wins As Integer
        Public Property max_damage_dealt As Integer
        Public Property wins As Integer
        Public Property losses As Integer
        Public Property damage_dealt As Integer
        Public Property max_planes_killed As Integer
        Public Property max_suppressions_count As Integer
        Public Property team_dropped_capture_points As Integer
        Public Property battles_since_512 As Integer
    End Class

    Public Class Main_Battery
        Public Property max_frags_battle As Integer
        Public Property frags As Integer
        Public Property hits As Integer
        Public Property shots As Integer
    End Class

    Public Class Second_Battery
        Public Property max_frags_battle As Integer
        Public Property frags As Integer
        Public Property hits As Integer
        Public Property shots As Integer
    End Class

    Public Class Ramming
        Public Property max_frags_battle As Integer
        Public Property frags As Integer
    End Class

    Public Class Torpedoes
        Public Property max_frags_battle As Integer
        Public Property frags As Integer
        Public Property hits As Integer
        Public Property shots As Integer
    End Class

    Public Class Aircraft
        Public Property max_frags_battle As Integer
        Public Property frags As Integer
    End Class

End Namespace
