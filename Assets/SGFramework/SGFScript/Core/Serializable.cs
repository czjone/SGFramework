namespace SGF {
	namespace Core {

		using System.Collections.Generic;
		using System.Collections;
		using System.IO;
		using System.Runtime.Serialization.Formatters.Binary;
		using System.Runtime.Serialization;
		using System;
		using UnityEngine;

		public class BinarySerializable<T> {
			private static byte[] Object2Bytes (object obj) {
				if (obj == null)
					throw new ArgumentNullException ("obj");
				byte[] buff;
				try {
					using (var ms = new MemoryStream ()) {
						IFormatter iFormatter = new BinaryFormatter ();
						iFormatter.Serialize (ms, obj);
						buff = ms.GetBuffer ();
					}
				} catch (Exception er) {
					throw new Exception (er.Message);
				}
				return buff;
			}

			private static bool Object2File (string name, object obj) {
				Stream flstr = null;
				BinaryWriter binaryWriter = null;
				try {
					flstr = new FileStream (name, FileMode.Create);
					binaryWriter = new BinaryWriter (flstr);
					var buff = Object2Bytes (obj);
					binaryWriter.Write (buff);
				} catch (Exception er) {
					throw new Exception (er.Message);
				} finally {
					if (binaryWriter != null) binaryWriter.Close ();
					if (flstr != null) flstr.Close ();
				}
				return true;
			}

			private static T Bytes2Object<T> (byte[] buff) {
				if (buff == null)
					throw new ArgumentNullException ("buff");

				if (buff.Length == 0)
					throw new System.Exception ("buff length is 0!");

				T obj;
				try {
					using (var ms = new MemoryStream (buff)) {
						ms.Seek (0, SeekOrigin.Begin);
						IFormatter iFormatter = new BinaryFormatter ();
						obj = (T) iFormatter.Deserialize (ms);
					}
				} catch (Exception er) {
					throw new Exception (er.Message);
				}
				return obj;
			}

			public static T LoadWithBytes (byte[] bytes) {
				return Bytes2Object<T> (bytes);
			}

			public static T LoadWithFile (string fname) {
				byte[] buff = System.IO.File.ReadAllBytes (fname);
				return LoadWithBytes (buff);
			}

			public void ToFile (string fname) {
				Object2File (fname, this);
			}

			public byte[] ToBytes () {
				return Object2Bytes (this);
			}
		}

	}
}