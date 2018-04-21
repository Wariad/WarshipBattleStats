
Imports Newtonsoft.Json
Imports System.Drawing

Namespace Data.Encyclopedia

    Public Class Ships
        Public Property status As String
        Public Property meta As New Data.Common.Meta
        Public Property [Error] As Data.Common.Error
        <JsonIgnore()>
        Public Property data As New List(Of Ship)
    End Class

    Public Class Ship
        Public Property tier As Integer
        Public Property description As String
        Public Property price_gold As Integer
        Public Property ship_id As Long
        Public Property name As String
        Public Property modules As Modules
        Public Property modules_tree As New List(Of TreeModule)
        Public Property nation As String
        Public Property is_premium As Boolean
        Public Property ship_id_str As String
        Public Property price_credit As Integer
        Public Property default_profile As Default_Profile
        Public Property upgrades As New List(Of Long)
        Public Property images As New Images
        Public Property type As String
        Public Property mod_slots As Integer
    End Class

    Public Class Modules
        Public Property engine As List(Of Long)
        Public Property artillery As List(Of Long)
        Public Property fighter As List(Of Object)
        Public Property hull As List(Of Long)
        Public Property torpedo_bomber As List(Of Object)
        Public Property torpedoes As List(Of Long)
        Public Property fire_control As List(Of Long)
        Public Property flight_control As List(Of Object)
        Public Property dive_bomber As List(Of Object)
    End Class

    Public Class TreeModule
        Public Property name As String
        Public Property next_modules As Object
        Public Property is_default As Boolean
        Public Property price_xp As Integer
        Public Property price_credit As Integer
        Public Property module_id As Long
        Public Property next_ships As Object
        Public Property type As String
        Public Property module_id_str As String
    End Class

    Public Class Default_Profile
        Public Property engine As Engine
        Public Property artillery As Artillery
        Public Property anti_aircraft As Anti_Aircraft
        Public Property mobility As Mobility
        Public Property hull As Hull
        Public Property atbas As Atbas
        Public Property torpedo_bomber As Object
        Public Property torpedoes As Torpedoes
        Public Property fire_control As Fire_Control
        Public Property fighters As Object
        Public Property weaponry As Weaponry
        Public Property battle_level_range_max As Integer
        Public Property battle_level_range_min As Integer
        Public Property flight_control As Object
        Public Property concealment As Concealment
        Public Property armour As Armour
        Public Property dive_bomber As Object
    End Class

    Public Class Engine
        Public Property max_speed As Double
        Public Property engine_id As Long
        Public Property engine_id_str As String
    End Class

    Public Class Artillery
        Public Property max_dispersion As Integer
        Public Property distance As Single
        Public Property shot_delay As Double
        Public Property rotation_time As Double
        Public Property artillery_id As Long
        Public Property shells As Shells
        Public Property artillery_id_str As String
        Public Property slots As Object
        Public Property gun_rate As Single
    End Class

    Public Class Shells
        Public Property AP As AP
        Public Property HE As HE
    End Class

    Public Class AP
        Public Property burn_probability As Double
        Public Property bullet_speed As Integer
        Public Property name As String
        Public Property damage As Integer
        Public Property bullet_mass As Integer
        Public Property type As String
    End Class

    Public Class HE
        Public Property burn_probability As Double
        Public Property bullet_speed As Integer
        Public Property name As String
        Public Property damage As Integer
        Public Property bullet_mass As Integer
        Public Property type As String
    End Class

    Public Class ArtillerySlot
        Public Property barrels As Integer
        Public Property name As String
        Public Property guns As Integer
    End Class

    Public Class Anti_Aircraft
        Public Property slots() As AntiAircraftSlot
        Public Property defense As Integer
    End Class

    Public Class AntiAircraftSlot
        Public Property distance As Double
        Public Property avg_damage As Integer
        Public Property caliber As Integer
        Public Property name As String
        Public Property guns As Integer
    End Class

    Public Class Mobility
        Public Property rudder_time As Double
        Public Property total As Double
        Public Property max_speed As Double
        Public Property turning_radius As Double
    End Class

    Public Class Hull
        Public Property hull_id As Long
        Public Property hull_id_str As String
        Public Property torpedoes_barrels As Integer
        Public Property anti_aircraft_barrels As Integer
        Public Property range As Range
        Public Property health As Integer
        Public Property planes_amount As Integer
        Public Property artillery_barrels As Integer
        Public Property atba_barrels As Integer
    End Class

    Public Class Range
        Public Property max As Integer
        Public Property min As Integer
    End Class

    Public Class Atbas
        Public Property distance As Single
        Public Property slots As Object
    End Class

    Public Class AtbasSlot
        Public Property burn_probability As Integer
        Public Property bullet_speed As Integer
        Public Property name As String
        Public Property shot_delay As Integer
        Public Property damage As Integer
        Public Property bullet_mass As Integer
        Public Property type As String
        Public Property gun_rate As Integer
    End Class

    Public Class Torpedoes
        Public Property visibility_dist As Single
        Public Property distance As Double
        Public Property torpedoes_id As Long
        Public Property torpedo_name As String
        Public Property reload_time As Integer
        Public Property max_damage As Integer
        Public Property rotation_time As Single
        Public Property torpedoes_id_str As String
        Public Property slots As Object
        Public Property torpedo_speed As Integer
    End Class

    Public Class TorpedoeSlot
        Public Property barrels As Integer
        Public Property caliber As Integer
        Public Property name As String
        Public Property guns As Integer
    End Class

    Public Class Fire_Control
        Public Property fire_control_id_str As String
        Public Property fire_control_id As Long
        Public Property distance_increase As Integer
        Public Property distance As Single
    End Class

    Public Class Weaponry
        Public Property torpedoes As Integer
        Public Property aircraft As Integer
        Public Property artillery As Integer
        Public Property anti_aircraft As Integer
    End Class

    Public Class Concealment
        Public Property total As Integer
        Public Property detect_distance_by_plane As Single
        Public Property detect_distance_by_ship As Single
    End Class

    Public Class Armour
        Public Property casemate As Casemate
        Public Property flood_prob As Integer
        Public Property deck As Deck
        Public Property flood_damage As Integer
        Public Property range As Range1
        Public Property health As Integer
        Public Property extremities As Extremities
        Public Property total As Integer
        Public Property citadel As Citadel
    End Class

    Public Class Casemate
        Public Property max As Integer
        Public Property min As Integer
    End Class

    Public Class Deck
        Public Property max As Integer
        Public Property min As Integer
    End Class

    Public Class Range1
        Public Property max As Integer
        Public Property min As Integer
    End Class

    Public Class Extremities
        Public Property max As Integer
        Public Property min As Integer
    End Class

    Public Class Citadel
        Public Property max As Integer
        Public Property min As Integer
    End Class

    Public Class Images
        Public Property small As String
        Public Property large As String
        Public Property medium As String
        Public Property contour As String
        <JsonIgnore()>
        Public Property smallImage As Image
        <JsonIgnore()>
        Public Property largeImage As Image
        <JsonIgnore()>
        Public Property mediumImage As Image
        <JsonIgnore()>
        Public Property contourImage As Image
    End Class

    Public Class Next_Ships

    End Class

    Public Class Achievments
        Public Property status As String
        Public Property meta As Meta
        Public Property data As New List(Of Achievment)
    End Class

    Public Class Meta
        Public Property count As Integer
    End Class

    Public Class Achievment
        Public Property battle_types As New List(Of String)
        Public Property multiple As Integer
        Public Property name As String
        Public Property count_per_battle As String
        Public Property max_progress As Integer
        Public Property is_progress As Integer
        Public Property image As String
        Public Property max_ship_level As Integer
        Public Property min_ship_level As Integer
        Public Property achievement_id As String
        Public Property image_inactive As String
        Public Property hidden As Integer
        Public Property reward As Boolean
        Public Property type As String
        Public Property sub_type As String
        Public Property description As String
        <JsonIgnore()>
        Public Property inactiveImage As Image
        <JsonIgnore()>
        Public Property activeImage As Image

    End Class

End Namespace


