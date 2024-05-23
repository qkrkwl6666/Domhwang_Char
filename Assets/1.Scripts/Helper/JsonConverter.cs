using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class ColorConverter : JsonConverter<Color>
{
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        Color c = new Color();
        c.r = (float)jo["r"];
        c.g = (float)jo["g"];
        c.b = (float)jo["b"];
        c.a = (float)jo["a"];
        return c;
    }

    public override void WriteJson(JsonWriter writer, Color value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("r");
        writer.WriteValue(value.r);
        writer.WritePropertyName("g");
        writer.WriteValue(value.g);
        writer.WritePropertyName("b");
        writer.WriteValue(value.b);
        writer.WritePropertyName("a");
        writer.WriteValue(value.a);
        writer.WriteEndObject();
    }
}


public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        Quaternion q = new Quaternion(
            (float)jo["x"],
            (float)jo["y"],
            (float)jo["z"],
            (float)jo["w"]);
        return q;
    }
    public override void WriteJson(JsonWriter writer, Quaternion value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WritePropertyName("w");
        writer.WriteValue(value.w);
        writer.WriteEndObject();
    }
}


public class Vector2Converter : JsonConverter<Vector2>
{
    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        Vector2 v = new Vector2();
        v.x = (float)jo["x"];
        v.y = (float)jo["y"];
        return v;
    }
    public override void WriteJson(JsonWriter writer, Vector2 value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WriteEndObject();
    }
}


public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        Vector3 v = new Vector3();
        v.x = (float)jo["x"];
        v.y = (float)jo["y"];
        v.z = (float)jo["z"];
        return v;
    }
    public override void WriteJson(JsonWriter writer, Vector3 value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}

public class CharacterInfoConverter : JsonConverter<CharacterInfo>
{
    public override CharacterInfo ReadJson(JsonReader reader, Type objectType, CharacterInfo existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        CharacterInfo v = new CharacterInfo();
        
        v.Id = (int)jo["Id"];
        v.Name = (string)jo["Name"];
        v.Tier = (string)jo["Tier"];
        v.Atk = (int)jo["Atk"];
        v.Atk_Up = (int)jo["Atk_Up"];
        v.Run = (int)jo["Run"];
        v.Run_Up = (int)jo["Run_Up"];
        v.Skill_Id = (int)jo["Skill_Id"];
        v.Level = (int)jo["Level"];
        v.InstanceId = (int)jo["InstanceId"];
        v.Texture = (int)jo["Texture"];
        v.Atk_Effect_Id = (int)jo["Atk_Effect_Id"];
        v.Run_Effect_Id = (int)jo["Run_Effect_Id"];
        v.Cry_Effect_Id = (int)jo["Cry_Effect_Id"];

        return v;
    }

    public override void WriteJson(JsonWriter writer, CharacterInfo value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Id");
        writer.WriteValue(value.Id);
        writer.WritePropertyName("Name");
        writer.WriteValue(value.Name);
        writer.WritePropertyName("Tier");
        writer.WriteValue(value.Tier);
        writer.WritePropertyName("Atk");
        writer.WriteValue(value.Atk);
        writer.WritePropertyName("Atk_Up");
        writer.WriteValue(value.Atk_Up);
        writer.WritePropertyName("Run");
        writer.WriteValue(value.Run);
        writer.WritePropertyName("Run_Up");
        writer.WriteValue(value.Run_Up);
        writer.WritePropertyName("Skill_Id");
        writer.WriteValue(value.Skill_Id);
        writer.WritePropertyName("Level");
        writer.WriteValue(value.Level);
        writer.WritePropertyName("InstanceId");
        writer.WriteValue(value.InstanceId);
        writer.WritePropertyName("Texture");
        writer.WriteValue(value.Texture);
        writer.WritePropertyName("Atk_Effect_Id");
        writer.WriteValue(value.Atk_Effect_Id);
        writer.WritePropertyName("Run_Effect_Id");
        writer.WriteValue(value.Run_Effect_Id);
        writer.WritePropertyName("Cry_Effect_Id");
        writer.WriteValue(value.Cry_Effect_Id);
        writer.WriteEndObject();
    }
}
