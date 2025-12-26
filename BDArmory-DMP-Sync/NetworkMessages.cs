using System;
using System.IO;

namespace BDArmoryDMPSync
{
    /// <summary>
    /// Network message types for BDArmory sync
    /// </summary>
    public enum MessageType : byte
    {
        BuildingDamage = 0,
        VesselDamage = 1,
        WeaponFire = 2,
        Explosion = 3
    }

    /// <summary>
    /// Building damage network message
    /// </summary>
    public class BuildingDamageMessage
    {
        public string BuildingId;
        public float DamageFraction;
        public string PlayerName;

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)MessageType.BuildingDamage);
                writer.Write(BuildingId ?? "");
                writer.Write(DamageFraction);
                writer.Write(PlayerName ?? "");
                return ms.ToArray();
            }
        }

        public static BuildingDamageMessage Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                byte msgType = reader.ReadByte();
                if (msgType != (byte)MessageType.BuildingDamage)
                    return null;

                return new BuildingDamageMessage
                {
                    BuildingId = reader.ReadString(),
                    DamageFraction = reader.ReadSingle(),
                    PlayerName = reader.ReadString()
                };
            }
        }
    }

    /// <summary>
    /// Vessel damage network message
    /// </summary>
    public class VesselDamageMessage
    {
        public Guid VesselId;
        public uint PartFlightId;
        public float Damage;
        public bool IsExplosive;
        public string PlayerName;

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)MessageType.VesselDamage);
                writer.Write(VesselId.ToByteArray());
                writer.Write(PartFlightId);
                writer.Write(Damage);
                writer.Write(IsExplosive);
                writer.Write(PlayerName ?? "");
                return ms.ToArray();
            }
        }

        public static VesselDamageMessage Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                byte msgType = reader.ReadByte();
                if (msgType != (byte)MessageType.VesselDamage)
                    return null;

                return new VesselDamageMessage
                {
                    VesselId = new Guid(reader.ReadBytes(16)),
                    PartFlightId = reader.ReadUInt32(),
                    Damage = reader.ReadSingle(),
                    IsExplosive = reader.ReadBoolean(),
                    PlayerName = reader.ReadString()
                };
            }
        }
    }

    /// <summary>
    /// Weapon fire network message (visual only)
    /// </summary>
    public class WeaponFireMessage
    {
        public Guid VesselId;
        public string WeaponName;
        public bool IsFiring;
        public string PlayerName;

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)MessageType.WeaponFire);
                writer.Write(VesselId.ToByteArray());
                writer.Write(WeaponName ?? "");
                writer.Write(IsFiring);
                writer.Write(PlayerName ?? "");
                return ms.ToArray();
            }
        }

        public static WeaponFireMessage Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                byte msgType = reader.ReadByte();
                if (msgType != (byte)MessageType.WeaponFire)
                    return null;

                return new WeaponFireMessage
                {
                    VesselId = new Guid(reader.ReadBytes(16)),
                    WeaponName = reader.ReadString(),
                    IsFiring = reader.ReadBoolean(),
                    PlayerName = reader.ReadString()
                };
            }
        }
    }

    /// <summary>
    /// Explosion network message
    /// </summary>
    public class ExplosionMessage
    {
        public float ExplosionPower;
        public Vector3d Position;
        public string PlayerName;

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((byte)MessageType.Explosion);
                writer.Write(ExplosionPower);
                writer.Write(Position.x);
                writer.Write(Position.y);
                writer.Write(Position.z);
                writer.Write(PlayerName ?? "");
                return ms.ToArray();
            }
        }

        public static ExplosionMessage Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                byte msgType = reader.ReadByte();
                if (msgType != (byte)MessageType.Explosion)
                    return null;

                return new ExplosionMessage
                {
                    ExplosionPower = reader.ReadSingle(),
                    Position = new Vector3d(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble()),
                    PlayerName = reader.ReadString()
                };
            }
        }
    }
}
