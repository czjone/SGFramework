// automatically generated by the FlatBuffers compiler, do not modify

package SGFComm;

import java.nio.*;
import java.lang.*;
import java.util.*;
import com.google.flatbuffers.*;

@SuppressWarnings("unused")public final class CL_GS_Login extends Table {
  public static CL_GS_Login getRootAsCL_GS_Login(ByteBuffer _bb) { return getRootAsCL_GS_Login(_bb, new CL_GS_Login()); }
  public static CL_GS_Login getRootAsCL_GS_Login(ByteBuffer _bb, CL_GS_Login obj) { _bb.order(ByteOrder.LITTLE_ENDIAN); return (obj.__assign(_bb.getInt(_bb.position()) + _bb.position(), _bb)); }
  public void __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; }
  public CL_GS_Login __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public String token() { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; }
  public ByteBuffer tokenAsByteBuffer() { return __vector_as_bytebuffer(4, 1); }
  public ByteBuffer tokenInByteBuffer(ByteBuffer _bb) { return __vector_in_bytebuffer(_bb, 4, 1); }

  public static int createCL_GS_Login(FlatBufferBuilder builder,
      int tokenOffset) {
    builder.startObject(1);
    CL_GS_Login.addToken(builder, tokenOffset);
    return CL_GS_Login.endCL_GS_Login(builder);
  }

  public static void startCL_GS_Login(FlatBufferBuilder builder) { builder.startObject(1); }
  public static void addToken(FlatBufferBuilder builder, int tokenOffset) { builder.addOffset(0, tokenOffset, 0); }
  public static int endCL_GS_Login(FlatBufferBuilder builder) {
    int o = builder.endObject();
    return o;
  }
}

