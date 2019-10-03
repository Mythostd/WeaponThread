using static WeaponThread.Session.ShieldDefinition.ShieldType;
using static WeaponThread.Session.AmmoTrajectory.GuidanceType;
using static WeaponThread.Session.HardPointDefinition.Prediction;
using static WeaponThread.Session.AreaDamage.AreaEffectType;
using static WeaponThread.Session.TargetingDefinition.BlockTypes;
using static WeaponThread.Session.TargetingDefinition.Threat;
using static WeaponThread.Session.Shrapnel.ShrapnelShape;
using static WeaponThread.Session.ShapeDefinition.Shapes;
using static WeaponThread.Session;

namespace WeaponThread
{   // Don't edit above this line
    partial class Weapons { WeaponDefinition Gatling => new WeaponDefinition {
    Assignments = new ModelAssignments
    {
        MountPoints = new[]
        {   
            MountPoint(subTypeId: "PDCTurretLB", aimPartId: "Boomsticks", muzzlePartId: "Boomsticks"),
            MountPoint(subTypeId: "PDCTurretSB", aimPartId: "Boomsticks", muzzlePartId: "Boomsticks"),
        },
        Barrels = Names("muzzle_barrel_001", "muzzle_barrel_002", "muzzle_barrel_003", "muzzle_barrel_004", "muzzle_barrel_005", "muzzle_barrel_006"),
        EnableSubPartPhysics = false
    },
    HardPoint = new HardPointDefinition
    {
        WeaponId = "Gatling", // name of weapon in terminal
        AmmoMagazineId = "Blank",
        Block = AimControl(trackTargets: true, turretAttached: true, turretController: true, rotateRate: 0.01f, elevateRate: 0.01f, offset: Vector(x: 0, y: 0, z: 0), fixedOffset: false, debug: true),
        DeviateShotAngle = 0f,
        AimingTolerance = 2f, // 0 - 180 firing angle
        EnergyCost = 0.00000000001f, //(((EnergyCost * DefaultDamage) * ShotsPerSecond) * BarrelsPerShot) * ShotsPerBarrel
        Hybrid = false, //projectile based weapon with energy cost
        EnergyPriority = 0, //  0 = Lowest shares power with shields, 1 = Medium shares power with thrusters and over powers shields, 2 = Highest Does not share power will use all available power until energy requirements met
        RotateBarrelAxis = 3, // 0 = off, 1 = xAxis, 2 = yAxis, 3 = zAxis
        AimLeadingPrediction = Advanced, // Off, Basic, Accurate, Advanced
        DelayCeaseFire = 0, // Measured in game ticks (6 = 100ms, 60 = 1 seconds, etc..).
        GridWeaponCap = 3, // 0 = unlimited, the smallest weapon cap assigned to a subTypeId takes priority.
        Ui = Display(rateOfFire: true, damageModifier: true, toggleGuidance: false, enableOverload: true),

        Loading = new AmmoLoading
        {
            RateOfFire = 3600,
            BarrelsPerShot = 6,
            TrajectilesPerBarrel = 1, // Number of Trajectiles per barrel per fire event.
            SkipBarrels = 0,
            ReloadTime = 600, // Measured in game ticks (6 = 100ms, 60 = 1 seconds, etc..).
            DelayUntilFire = 0, // Measured in game ticks (6 = 100ms, 60 = 1 seconds, etc..).
            HeatPerShot = 1, //heat generated per shot
            MaxHeat = 37000, //max heat before weapon enters cooldown (70% of max heat)
            Cooldown = .95f, //percent of max heat to be under to start firing again after overheat accepts .2-.95
            HeatSinkRate = 200, //amount of heat lost per second
            DegradeRof = false, // progressively lower rate of fire after 80% heat threshold (80% of max heat)
            ShotsInBurst = 0,
            DelayAfterBurst = 0, // Measured in game ticks (6 = 100ms, 60 = 1 seconds, etc..).
        },
    },
    Targeting = new TargetingDefinition
    {
        Threats = Valid(Characters, Projectiles, Grids),
        SubSystems = Priority(Thrust, Utility, Offense, Power, Production, Any), //define block type targeting order
        ClosestFirst = false, // tries to pick closest targets first (blocks on grids, projectiles, etc...).
        MinimumDiameter = 10, // 0 = unlimited, Minimum radius of threat to engage.
        MaximumDiameter = 0, // 0 = unlimited, Maximum radius of threat to engage.
        TopTargets = 4, // 0 = unlimited, max number of top targets to randomize between.
        TopBlocks = 4, // 0 = unlimited, max number of blocks to randomize between
        StopTrackingSpeed = 1000, // do not track target threats traveling faster than this speed
    },
    DamageScales = new DamageScaleDefinition
    {
        MaxIntegrity = 0f, // 0 = disabled, 1000 = any blocks with currently integrity above 1000 will be immune to damage.
        DamageVoxels = true, // true = voxels are vulnerable to this weapon
        SelfDamage = false, // true = allow self damage.

        // modifier values: -1 = disabled (higher performance), 0 = no damage, 0.01 = 1% damage, 2 = 200% damage.
        Characters = -1f,
        Grids = Options(largeGridModifier: -1f, smallGridModifier: -1f),
        Armor = Options(armor: -1f, light: -1f, heavy: -1f, nonArmor: -1f),
        Shields = Options(modifier: 0.1f, type: Kinetic), // Types: Kinetic, Energy, Emp or Bypass

        // first true/false (ignoreOthers) will cause projectiles to pass through all blocks that do not match the custom subtypeIds.
        Custom = SubTypeIds(false, Block(subTypeId: "Test1", modifier: -1), Block(subTypeId: "Test2", modifier: -1)),
    },
    Ammo = new AmmoDefinition
    {
        BaseDamage = 0f,
        Mass = 0f, // in kilograms
        Health = 0, // 0 = disabled, otherwise how much damage it can take from other trajectiles before dying.
        BackKickForce = 0f,
        Shape = Options(shape: Line, diameter: 0), //defines the collision shape of projectile, defaults line and visual Line Length if set to 0
        ObjectsHit = Options(maxObjectsHit: 0, countBlocks: false), // 0 = disabled, value determines max objects (and/or blocks) penetrated per hit
        Shrapnel = Options(baseDamage: 1, fragments: 0, maxTrajectory: 100, noAudioVisual: true, noGuidance: true, shape: HalfMoon),

        AreaEffect = new AreaDamage
        {
            AreaEffect = AnchorField, // Disabled = do not use area effect at all, Explosive, Radiant, AntiSmart, JumpNullField, JumpNullField, EnergySinkField, AnchorField, EmpField, OffenseField, NavField, DotField.
            AreaEffectDamage = 10f, // 0 = use spillover from BaseDamage, otherwise use this value.
            AreaEffectRadius = 0f,
            Pulse = Options(interval: 60, pulseChance: 15), // interval measured in game ticks (60 == 1 second), pulseChance chance (0 - 100) that an entity in field will be hit
            Explosions = Options(noVisuals: false, noSound: false, scale: 1, customParticle: "", customSound: ""),
            Detonation = Options(detonateOnEnd: true, armOnlyOnHit: false, detonationDamage: 999999, detonationRadius: 75),
            EwarFields = Options(duration: 600, stackDuration: true, depletable: true)
        },
        Beams = new BeamDefinition
        {
            Enable = true,
            VirtualBeams = true, // Only one hot beam, but with the effectiveness of the virtual beams combined (better performace)
            ConvergeBeams = false, // When using virtual beams this option visually converges the beams to the location of the real beam.
            RotateRealBeam = true, // The real (hot beam) is rotated between all virtual beams, instead of centered between them.
            OneParticle = true, // Only spawn one particle hit per beam weapon.
        },
        Trajectory = new AmmoTrajectory
        {
            Guidance = None,
            TargetLossDegree = 80f,
            TargetLossTime = 0, // 0 is disabled, Measured in game ticks (6 = 100ms, 60 = 1 seconds, etc..).
            AccelPerSec = 0f,
            DesiredSpeed = 0f,
            MaxTrajectory = 5000f,
            FieldTime = 0, // 0 is disabled, a value causes the projectile to come to rest, spawn a field and remain for a time (Measured in game ticks, 60 = 1 second)
            SpeedVariance = Random(start: 0, end: 0), // subtracts value from DesiredSpeed
            RangeVariance = Random(start: 0, end: 0), // subtracts value from MaxTrajectory
            Smarts = new Smarts
            {
                Inaccuracy = 0f, // 0 is perfect, hit accuracy will be a random num of meters between 0 and this value.
                Aggressiveness = 1f, // controls how responsive tracking is.
                MaxLateralThrust = 0.5, // controls how sharp the trajectile may turn
                TrackingDelay = 0, // Measured in Shape diameter units traveled.
                MaxChaseTime = 1800, // Measured in game ticks (6 = 100ms, 60 = 1 seconds, etc..).
            },
            Mines = Options(detectRadius: 200, deCloakRadius: 100, fieldTime: 1800, cloak: true, persist: false),
        },
    },
    Graphics = new GraphicDefinition
    {
        ModelName = "",
        VisualProbability = 1f,
        ShieldHitDraw = true,
        Particles = new ParticleDefinition
        {
            Ammo = new Particle
            {
                Name = "", //ShipWelderArc
                Color = Color(red: 128, green: 0, blue: 0, alpha: 32),
                Offset = Vector(x: 0, y: -1, z: 0),
                Extras = Options(loop: true, restart: false, distance: 5000, duration: 1, scale: 1)
            },
            Hit = new Particle
            {
                Name = "ShipWelderArc",
                Color = Color(red: 243, green: 190, blue: 51, alpha: 1),
                Offset = Vector(x: 0, y: 0, z: 0),
                Extras = Options(loop: false, restart: false, distance: 5000, duration: 1, scale: 1.5f),
            },
            Barrel1 = new Particle
            {
                Name = "", // Smoke_LargeGunShot
                Color = Color(red: 255, green: 0, blue: 0, alpha: 1),
                Offset = Vector(x: 0, y: -1, z: 0),
                Extras = Options(loop: false, restart: false, distance: 50, duration: 6, scale: 1f),
            },
            Barrel2 = new Particle
            {
                Name = "",//Muzzle_Flash_Large
                Color = Color(red: 10, green: 0, blue: 0, alpha: 1),
                Offset = Vector(x: 0, y: -1, z: 0),
                Extras = Options(loop: false, restart: false, distance: 150, duration: 6, scale: 1f),
            },
        },
        Line = new LineDefinition
        {
            Tracer = Base(enable: true, length: 1f, width: 0.035f, color: Color(red: 8, green: 8, blue: 8, alpha: 8)),
            TracerMaterial = "WeaponLaser", // WeaponLaser, ProjectileTrailLine, WarpBubble, etc..
            ColorVariance = Random(start: 0.75f, end: 2f), // multiply the color by random values within range.
            WidthVariance = Random(start: 0f, end: 0.015f), // adds random value to default width (negatives shrinks width)
            Trail = Options(enable: false, material: "WeaponLaser", decayTime: 60, color: Color(red: 16, green: 16, blue: 64, alpha: 8)),
            OffsetEffect = Options(maxOffset: 0, minLength: 5, maxLength: 15), // 0 offset value disables this effect
        },
    },
    Audio = new AudioDefinition
    {
        HardPoint = new AudioHardPointDefinition
        {
            FiringSound = "", // WepShipGatlingShot
            FiringSoundPerShot = true,
            ReloadSound = "",
            NoAmmoSound = "",
            HardPointRotationSound = "WepTurretGatlingRotate",
            BarrelRotationSound = "WepShipGatlingRotation",
        },

        Ammo = new AudioAmmoDefinition
        {
            TravelSound = "",
            HitSound = "",
        }, // Don't edit below this line
    },
};}}