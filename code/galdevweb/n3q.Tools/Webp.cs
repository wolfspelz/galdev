#nullable enable
using System;
using System.Data;

namespace n3q.Tools
{
    public static class Webp {
        // WebP container format:
        // https://developers.google.com/speed/webp/docs/riff_container

        public static int GetDurationMSec(byte[] data)
        {
            var chunks = new WebpParser(data);
            var duration = 0;

            string chunkType;
            BytesParser chunkData;

            chunks.ParseHead();
            if (!chunks.AnimatedBit) {
                return 0;
            }

            // Parse global animation chunk:
            int loopCount = 1;
            bool animParsed = false;
            while (!animParsed) {
                (chunkType, chunkData) = chunks.NextChunk();
                switch (chunkType) {
                    case "ANIM": // Global animation data chunk.
                        loopCount = chunkData.GetUInt16(4);
                        animParsed = true;
                    break;
                    case "ANMF": // Premature animation frame chunk.
                    case "ALPH": // Alpha chunk outside animation frame chunk.
                    case "VP8 ": // Lossy content bitstream chunk outside animation frame chunk.
                    case "VP8L": { // Lossless content bitstream chunk outside animation frame chunk.
                        throw new SyntaxErrorException("Unexpected Chunk found!");
                    }
                    default:
                        // Ignore unknown chunk.
                    break;
                }
            }

            // Parse global animation chunk:
            while (!chunks.IsEmpty()) {
                (chunkType, chunkData) = chunks.NextChunk();
                switch (chunkType) {
                    case "ANMF": // Animation frame chunk.
                        duration += chunkData.GetUInt16(12);
                    break;
                    case "ANIM": // Extra global animation data chunk.
                    case "ALPH": // Alpha chunk outside animation frame chunk.
                    case "VP8 ": // Lossy content bitstream chunk outside animation frame chunk.
                    case "VP8L": { // Lossless content bitstream chunk outside animation frame chunk.
                        throw new SyntaxErrorException("Unexpected Chunk found!");
                    }
                    default:
                        // Ignore unknown chunk.
                    break;
                }
            }

            // Apply loop count:
            if (loopCount > 0) {
                duration *= loopCount;
            }

            return duration;
        }

        public class WebpParser: BytesParser {

            public string Format = "";
            public bool Extended = false;
            public bool Lossless = false;
            public bool AnimatedBit = false;
            public bool AlphaBit = false;

            public WebpParser(byte[] bytes, int offset = 0, int? count = null):
            base(bytes, Endianness.LittleEndian, offset, count)
            {
            }

            public void ParseHead() {
                if (NextAsciiString(4) != "RIFF") {
                    throw new SyntaxErrorException("Invalid RIFF header: Doesn't start with RIFF!");
                }
                if (NextUInt32() != GetLength()) {
                    throw new SyntaxErrorException("Invalid RIFF header: Chunk length doesn't match remaining file length!");
                }
                if (NextAsciiString(4) != "WEBP") {
                    throw new SyntaxErrorException("Invalid RIFF header: First chunk's payload isn't WEBP!");
                }
                var (type, chunk) = NextChunk();
                Format = type;
                switch (type) {
                    case "VP8 ":
                        
                    break;
                    case "VP8L":
                        Lossless = true;
                    break;
                    case "VP8X":
                        Extended = true;
                        AnimatedBit = chunk.GetBit(0, 1);
                        AlphaBit = chunk.GetBit(0, 4);
                    break;
                    default:
                        throw new SyntaxErrorException("Second chunk's type is {type}!");
                }
            }

            public Tuple<string, BytesParser> NextChunk() {
                var type = NextAsciiString(4);
                var dataLength = NextUInt32();
                var parser = NextBytesParser(dataLength);
                Skip(dataLength % 2); // Skip padding byte for odd payload length.
                return new Tuple<string, BytesParser>(type, parser);
            }

        }
    }
}
