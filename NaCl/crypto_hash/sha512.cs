﻿using System;

namespace UCIS.NaCl.crypto_hash {
	public static class sha512 {
		public static int BYTES = 64;

/*		static Byte[] iv = new Byte[64] {
  0x6a,0x09,0xe6,0x67,0xf3,0xbc,0xc9,0x08,
  0xbb,0x67,0xae,0x85,0x84,0xca,0xa7,0x3b,
  0x3c,0x6e,0xf3,0x72,0xfe,0x94,0xf8,0x2b,
  0xa5,0x4f,0xf5,0x3a,0x5f,0x1d,0x36,0xf1,
  0x51,0x0e,0x52,0x7f,0xad,0xe6,0x82,0xd1,
  0x9b,0x05,0x68,0x8c,0x2b,0x3e,0x6c,0x1f,
  0x1f,0x83,0xd9,0xab,0xfb,0x41,0xbd,0x6b,
  0x5b,0xe0,0xcd,0x19,0x13,0x7e,0x21,0x79
};*/

		public static unsafe void crypto_hash(Byte[] outv, Byte[] inv, int inlen) {
			if (outv.Length < 64) throw new ArgumentException("outv.Length < 64");
			if (inv.Length < inlen) throw new ArgumentException("inv.Length < inlen");
			fixed (Byte* outp = outv, inp = inv) crypto_hash(outp, inp, (UInt64)inlen);
		}
		public static unsafe void crypto_hash(Byte* outp, Byte* inp, UInt64 inlen) {
//			Byte[] h = new Byte[64];
			Byte[] padded = new Byte[256];
			UInt64 i;
			UInt64 bytes = inlen;
			Byte[] h = new Byte[64] {
  0x6a,0x09,0xe6,0x67,0xf3,0xbc,0xc9,0x08,
  0xbb,0x67,0xae,0x85,0x84,0xca,0xa7,0x3b,
  0x3c,0x6e,0xf3,0x72,0xfe,0x94,0xf8,0x2b,
  0xa5,0x4f,0xf5,0x3a,0x5f,0x1d,0x36,0xf1,
  0x51,0x0e,0x52,0x7f,0xad,0xe6,0x82,0xd1,
  0x9b,0x05,0x68,0x8c,0x2b,0x3e,0x6c,0x1f,
  0x1f,0x83,0xd9,0xab,0xfb,0x41,0xbd,0x6b,
  0x5b,0xe0,0xcd,0x19,0x13,0x7e,0x21,0x79
			};

//			for (i = 0; i < 64; ++i) h[i] = iv[i];

			fixed (Byte* hp = h) crypto_hashblocks.sha512.crypto_hashblocks(hp, inp, inlen);
			inp += inlen;
			inlen &= 127;
			inp -= inlen;

			for (i = 0; i < inlen; ++i) padded[i] = inp[i];
			padded[inlen] = 0x80;

			if (inlen < 112) {
				for (i = inlen + 1; i < 119; ++i) padded[i] = 0;
				padded[119] = (Byte)(bytes >> 61);
				padded[120] = (Byte)(bytes >> 53);
				padded[121] = (Byte)(bytes >> 45);
				padded[122] = (Byte)(bytes >> 37);
				padded[123] = (Byte)(bytes >> 29);
				padded[124] = (Byte)(bytes >> 21);
				padded[125] = (Byte)(bytes >> 13);
				padded[126] = (Byte)(bytes >> 5);
				padded[127] = (Byte)(bytes << 3);
				fixed (Byte* hp = h, paddedp = padded) crypto_hashblocks.sha512.crypto_hashblocks(hp, paddedp, 128);
			} else {
				for (i = inlen + 1; i < 247; ++i) padded[i] = 0;
				padded[247] = (Byte)(bytes >> 61);
				padded[248] = (Byte)(bytes >> 53);
				padded[249] = (Byte)(bytes >> 45);
				padded[250] = (Byte)(bytes >> 37);
				padded[251] = (Byte)(bytes >> 29);
				padded[252] = (Byte)(bytes >> 21);
				padded[253] = (Byte)(bytes >> 13);
				padded[254] = (Byte)(bytes >> 5);
				padded[255] = (Byte)(bytes << 3);
				fixed (Byte* hp = h, paddedp = padded) crypto_hashblocks.sha512.crypto_hashblocks(hp, paddedp, 256);
			}

			for (i = 0; i < 64; ++i) outp[i] = h[i];
		}
	}
}
