// automatically generated by the FlatBuffers compiler, do not modify

package SGFComm;

import java.nio.*;
import java.lang.*;
import java.util.*;
import com.google.flatbuffers.*;

@SuppressWarnings("unused")public final class LS_CL_ServerList extends Table {
  public static LS_CL_ServerList getRootAsLS_CL_ServerList(ByteBuffer _bb) { return getRootAsLS_CL_ServerList(_bb, new LS_CL_ServerList()); }
  public static LS_CL_ServerList getRootAsLS_CL_ServerList(ByteBuffer _bb, LS_CL_ServerList obj) { _bb.order(ByteOrder.LITTLE_ENDIAN); return (obj.__assign(_bb.getInt(_bb.position()) + _bb.position(), _bb)); }
  public void __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; }
  public LS_CL_ServerList __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Server Servers(int j) { return Servers(new Server(), j); }
  public Server Servers(Server obj, int j) { int o = __offset(4); return o != 0 ? obj.__assign(__indirect(__vector(o) + j * 4), bb) : null; }
  public int ServersLength() { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; }

  public static int createLS_CL_ServerList(FlatBufferBuilder builder,
      int ServersOffset) {
    builder.startObject(1);
    LS_CL_ServerList.addServers(builder, ServersOffset);
    return LS_CL_ServerList.endLS_CL_ServerList(builder);
  }

  public static void startLS_CL_ServerList(FlatBufferBuilder builder) { builder.startObject(1); }
  public static void addServers(FlatBufferBuilder builder, int ServersOffset) { builder.addOffset(0, ServersOffset, 0); }
  public static int createServersVector(FlatBufferBuilder builder, int[] data) { builder.startVector(4, data.length, 4); for (int i = data.length - 1; i >= 0; i--) builder.addOffset(data[i]); return builder.endVector(); }
  public static void startServersVector(FlatBufferBuilder builder, int numElems) { builder.startVector(4, numElems, 4); }
  public static int endLS_CL_ServerList(FlatBufferBuilder builder) {
    int o = builder.endObject();
    return o;
  }
}

