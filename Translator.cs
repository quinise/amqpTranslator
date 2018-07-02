using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace translator{
    public class Translator{
        private int index = 0;
        private const byte nullValue = 0x40;
        private const byte list0 = 0x45;
        private const byte list8 = 0xc0;
        private const byte list32 = 0xd0;
        private const byte map8 = 0xc1;
        private const byte map32 = 0xd1;
        private const byte uInt0 = 0x43;
        private const byte uInt8 = 0x52;
        private const byte uInt32 = 0x70;
        private const byte str8Utf = 0xa1;
        private const byte str32Utf = 0xb1;
        private const byte uShort16 = 0x60;
        private const byte sym8 = 0xa3;
        private const byte sym32 = 0xb3;
        private const byte bool0 = 0x42;
        private const byte bool1 = 0x41;
        private const byte bool2 = 0x56;
        private const byte uByte0 = 0x50;
        private const byte uLong0 = 0x44;
        private const byte uLong8 = 0x53;
        private const byte uLong64 = 0x80;
        private const byte byte8 = 0x51;
        private const byte short16 = 0x61;
        private const byte int8 = 0x54;
        private const byte int32 = 0x71;
        private const byte long8 = 0x55;
        private const byte long64 = 0x81;
        private const byte float32 = 0x72;
        private const byte double64 = 0x82;
        private const byte decimal32 = 0x74;
        private const byte decimal64 = 0x84;
        private const byte decimal128 = 0x94;
        private const byte char32 = 0x73;
        private const byte timestamp64 = 0x83;
        private const byte uuid16 = 0x98;
        private const byte bin8 = 0x83;
        private const byte bin32 = 0xa0;
        private const byte array8 = 0xb0;
        private const byte array32 = 0xf0;
        private const byte openPerformative = 0x10;
        private const byte beginPerformative = 0x11;
        private const byte attachPerformative = 0x12;
        private const byte flowPerformative = 0x13;
        private const byte transferPerformative = 0x14;
        private const byte dispositionPerformative = 0x15;
        private const byte detachPerformative = 0x16;
        private const byte endPerformative = 0x17;
        private const byte closePerformative = 0x18;

        public void addToArray(ref byte destination, byte source, int index){
            if (index % 2 == 0){
                source = (byte)(source << 4);
            }
            destination |= source;  
        }
        public int getInt8(byte[] source) {
            return source[0];
        }
        public int getInt16(byte[] source) {
            byte[] destination = new Byte[2];
            Array.Copy(source, destination, 2);
            Array.Reverse(destination);
            return BitConverter.ToInt16(destination, 0);
        }
        public int getInt32(byte[] source){
            byte[] destination = new Byte[4];
            Array.Copy(source, destination, 4);
            Array.Reverse(destination);
            return BitConverter.ToInt32(destination, 0);
        }
        public long getInt64(byte[] source){
            byte[] destination = new Byte[8];
            Array.Copy(source, destination, 8);
            Array.Reverse(destination);
            return BitConverter.ToInt64(destination, 0);
        }
        public uint getUInt8(byte[] source) {
            return source[0];
        }
        public uint getUInt32(byte[] source){
            byte[] destination = new Byte[4];
            Array.Copy(source, destination, 4);
            Array.Reverse(destination);
            return BitConverter.ToUInt32(destination, 0);
        }
        public uint getUInt16(byte[] source){
            byte[] destination = new Byte[2];
            Array.Copy(source, destination, 2);
            Array.Reverse(destination);
            return BitConverter.ToUInt16(destination, 0);
        }
        public ulong getLong8(byte[] source){
            return source[0];
        }
        public ulong getULong8(byte[] source){
            byte[] destination = new Byte[0];
            Array.Copy(source, destination, 0);
            return BitConverter.ToUInt16(destination, 0);
        }
        public long getLong64(byte[] source) {
            byte[] destination = new Byte[8];
            Array.Copy(source, destination, 8);
            Array.Reverse(destination);
            return BitConverter.ToInt64(destination, 0);
        }
       public ulong getULong64(byte[] source){
            byte[] destination = new Byte[8];
            Array.Copy(source, destination, 8);
            Array.Reverse(destination);
            return BitConverter.ToUInt64(destination, 0);
        }

        public double getFloat32(byte[] source){
            byte[] destination = new Byte[4];
            Array.Copy(source, destination, 4);
            Array.Reverse(destination);
            return BitConverter.ToDouble(destination, 0);
        }

        public double getDouble64(byte[] source){
            byte[] destination = new Byte[8];
            Array.Copy(source, destination, 8);
            Array.Reverse(destination);
            return BitConverter.ToDouble(destination, 0);
        }
        public double getDecimal32(byte[] source){
            byte[] destination = new Byte[4];
            Array.Copy(source, destination, 4);
            Array.Reverse(destination);
            return BitConverter.ToDouble(destination, 0);
        }
        public double getDecimal64(byte[] source){
            byte[] destination = new Byte[8];
            Array.Copy(source, destination, 8);
            Array.Reverse(destination);
            return BitConverter.ToDouble(destination, 0);
        }
        public double getDecimal128(byte[] source){
            byte[] destination = new Byte[16];
            Array.Copy(source, destination, 16);
            Array.Reverse(destination);
            return BitConverter.ToDouble(destination, 0);
        }
        public char getChar32(byte[] source){
            byte[] destination = new Byte[4];
            Array.Copy(source, destination, 4);
            Array.Reverse(destination);
            return BitConverter.ToChar(destination, 0);
        }
        public int getIndex(){
            return index;
        }
        public void setIndex(int i){
            index = i;
        }
        public void findBool(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();

            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + " not present"));
                i++;
            } else if (source[i] == bool2 ){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "boolean with the octet 0x00 being false and octet 0x01 being true"));
                i++;
            } else if (source[i] == bool0){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": sender boolean value false"));
                i++;
            } else if (source[i] == bool1){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": receiver boolean value true"));
                i++;
            }
            setIndex(i);
        }
        public void findUbyte(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + " not present"));
                i++;
            } else if (source[i] == uByte0 ){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "8-bit unsigned integer"));
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + getUInt16(source.Skip(i).Take(2).ToArray())));
                i++;
            }
            setIndex(i);
        }
        public void findUshort(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();

            if (source[i] == nullValue) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value " + propertyName + " not present")); 
            } else if (source[i] == uShort16){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "16-bit unsigned int "));
                currentOutputList.Add(Tuple.Create("data", propertyName + getUInt16(source.Skip(i).Take(2).ToArray())));
                i += 2;
            } 
            setIndex(i);
        }

        public void findUint(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();

            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            
             } else if(source[i] == uInt32 || propertyName == "transfer number"){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "4 byte wide int"));
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + getUInt32(source.Skip(i).Take(4).ToArray())));
                i += 4;

            } else if (source[i] == uInt8) {
                i++;                
                currentOutputList.Add(Tuple.Create("metadata", "byte wide frame, max frame size: " + getUInt16(source.Skip(i).Take(2).ToArray())));
                i += 2;

            } else if (source[i] == uInt0) {
                i++;                
                currentOutputList.Add(Tuple.Create("metadata", "0 byte wide int " + propertyName));
                if (propertyName == "idle timeout"){
                    currentOutputList.Add(Tuple.Create("data", "Idle Timeout: 0 milliseconds"));
                }
            }
            setIndex(i);
        }
        public void findUlong(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();

            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            
             } else if(source[i] == uLong64){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "8 byte wide, unisgned long"));
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + getULong64(source.Skip(i).Take(8).ToArray())));
                i += 8;

            } else if (source[i] == uLong8) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "1 byte wide, unisgned long"));
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + getULong8(source.Skip(i).Take(8).ToArray())));
                i ++;

            } else if (source[i] == uLong0) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "0 byte wide int, unsigned long "));
                currentOutputList.Add(Tuple.Create("data", propertyName + " has the ulong value 0"));
            }
            setIndex(i);
        }

        public void findByte(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == byte8){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ", an 8-bit two's compliment integer in network byte order: " + getInt8(source.Skip(i).Take(1).ToArray())));
                i++;
            }
            setIndex(i);
        }

        public void findShort(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == short16) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ", an 16-bit two's compliment integer network byte order: " + getInt16(source.Skip(i).Take(2).ToArray())));
                i += 2;
            }
            setIndex(i);
        }

        public void findInt(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == int32) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ", an 32-bit two's compliment integer in network byte order: " + getInt32(source.Skip(i).Take(4).ToArray())));
                i += 4;
            } else if (source[i] == int8) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ", an 8-bit two's compliment integer in network byte order: " + getInt8(source.Skip(i).Take(1).ToArray())));
                i++;
            }
            setIndex(i);
        }

        public void findLong(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present"));
            } else if (source[i] == long64) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ", an 64-bit two's compliment integer in network byte order: " + getLong64(source.Skip(i).Take(8).ToArray())));
                i += 8;
            } else if (source[i] == long8) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ", an 8-bit two's compliment integer in network byte order: " + getInt8(source.Skip(i).Take(1).ToArray())));
                i++;
            }
            setIndex(i);
        }

        public void findFloat(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == float32){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + getFloat32(source.Skip(i).Take(4).ToArray()) + ": float" )); 
                i += 4;
            }
            setIndex(i);
        }

        public void findDouble(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            
            } else if (source[i] == double64){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + getDouble64(source.Skip(i).Take(8).ToArray()) + ": double" )); 
                i += 8;
            }
            setIndex(i);
        }

        public void findDecimal(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == decimal32) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "decimal32 using the Binary Integer Decimal encoding")); 
                currentOutputList.Add(Tuple.Create("data", propertyName + getDecimal32(source.Skip(i).Take(4).ToArray()) + ": decimal")); 
                i += 4;
            } else if (source[i] == decimal64) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "decimal64 using the Binary Integer Decimal encoding")); 
                currentOutputList.Add(Tuple.Create("data", propertyName + getDecimal64(source.Skip(i).Take(8).ToArray()) + ": decimal")); 
                i += 8;

            } else if (source[i] == decimal128) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "decimal128 using the Binary Integer Decimal encoding")); 
                currentOutputList.Add(Tuple.Create("data", propertyName + getDecimal32(source.Skip(i).Take(2).ToArray()) + ": decimal")); 
                i += 2;
            }
            setIndex(i);
        }

        public void findChar(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present"));  
            } else if (source[i] == char32) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "a UTF-32BE encoded Unicode character"));  
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + getChar32(source.Skip(i).Take(4).ToArray())));  
                i += 4;
            }
            setIndex(i);
        }

        public void findTimestamp(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == timestamp64) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "64-bit two's-complement integer representing milliseconds since the unix epoch"));  
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + getInt64(source.Skip(i).Take(8).ToArray())));  
                i += 8;
            }
            setIndex(i);
        }

        public void findUUId(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present"));
            } else if (source[i] == uuid16) {
                i++;
                //change ToString of source(i) to a method call that gets a sub array out of source
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + source[i].ToString("X2")));  
                i += 2;
            }
            
            setIndex(i);
        }

        public void findBinary(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == bin32) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + source[i].ToString("X2")));  
                i += 4;

            } else if (source[i] == bin8) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + source[i].ToString("X2")));
                i++;
            }
            setIndex(i);
        }
        public void findString(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
          int i = getIndex();

            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + " not present"));

            } else if (source[i] == str8Utf) {
                i++;
                int length = source[i];
                i++;
                byte[] hostname = source.Skip(i).Take(length).ToArray();
                currentOutputList.Add(Tuple.Create("metadata", "String descriptor length can fit into 4 bytes. Length is : " + length));                
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + System.Text.Encoding.UTF8.GetString(hostname, 0, length)));
                i = i + length;
            } else if (source[i] == str32Utf){
                 i++;
                int length = getInt32(source.Skip(1).Take(4).ToArray());
                i += 4;  
                byte[] hostname = source.Skip(i).Take(length).ToArray();
                i = i + length;
                currentOutputList.Add(Tuple.Create("metadata", "String descriptor length can fit into 4 bytes. Length is : " + length));
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + System.Text.Encoding.UTF8.GetString(hostname, 0, length)));
            }
            setIndex(i);
        }
        public void findSymbol(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();
            
            if (source[i] == nullValue) {
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value " + propertyName + " not present")); 
            } else if (source[i] == sym8){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "symbol with 1-byte length"));
                int numCharInSymbol = source[i];
                i++; 
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol)));
                i += numCharInSymbol;

            } else if (source[i] == sym32){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "symbol with 4-byte length"));
                int numCharInSymbol = getInt32(source.Skip(i).Take(4).ToArray());                
                i += 4;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol)));
                i += numCharInSymbol;
            }

            setIndex(i);
        }

        public void findList(List<Tuple<string, string>> currentOutputList, byte[] source) {
            int i = getIndex();
            int numElementsInList = 0;
            byte listType = source[i];
            currentOutputList.Add(Tuple.Create("metadata", "List Type: " + listType.ToString("X2")));
            i++;

            switch (listType) {
                case list0:
                    currentOutputList.Add(Tuple.Create("data","List 0: no elements in open list"));
                    numElementsInList = 0;
                    i++;
                    break;
                case list8:
                    currentOutputList.Add(Tuple.Create("metadata", "List 8, Number bytes wide: " + source[i]));
                    i++;
                    numElementsInList = source[i];
                    currentOutputList.Add(Tuple.Create("data", "There are " + numElementsInList + " elements in the list" ));
                    i++;
                    break;
                case list32:
                    int numBytesInList = getInt32(source.Skip(1).Take(4).ToArray());
                    i += 4;
                    currentOutputList.Add(Tuple.Create("data", "List 32, Number of bytes wide: " + numBytesInList));
                    numElementsInList = getInt32(source.Skip(1).Take(4).ToArray());
                    currentOutputList.Add(Tuple.Create("data","There are " + numElementsInList + " in the list" ));
                    i += 4;
                    break;
                default:
                    currentOutputList.Add(Tuple.Create("metadata", "Invalid list"));
                    break;
            }
            
            setIndex(i);
        }

        //revisit to access key value pairs
        public void findMap(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName){
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + " not present"));
            } else if (source[i] == map8) {
                i++;
                var mapLength = source[i];
                currentOutputList.Add(Tuple.Create("metadata", "1 byte length,  " + mapLength + " bytes long"));
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + BitConverter.ToString(source.Skip(i).Take(mapLength).ToArray())));
                i += mapLength;
            } else if (source[i] == map32) {
                i++;
                var mapLength = getInt32(source.Skip(i).Take(4).ToArray());
                currentOutputList.Add(Tuple.Create("metadata", "Map is 1 byte length,  " + mapLength + " bytes long"));
                i += 4;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + BitConverter.ToString(source.Skip(i).Take(mapLength).ToArray())));
                i += mapLength;
            }
            setIndex(i);
        }

        // private void findMapElements(List<Tuple<string, string>> currentOutputList, byte[] source) {
        //     for(int k = 0; k < mapLength; k++) {


        //     }
        // }
        
        public void findArray(List<Tuple<string, string>> currentOutputList, byte[] source, string propertyName) {
            int i = getIndex();
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add(Tuple.Create("metadata", "null value, " + propertyName + " not present")); 
            } else if (source[i] == array32) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + source[i].ToString("X2")));  
                i += 4;
            } else if (source[i] == array8) {
                i++;
                currentOutputList.Add(Tuple.Create("data", propertyName + ": " + source[i].ToString("X2")));  
                i++;

            }
            
            setIndex(i);
        }
        public string getSenderSettleMode(List<Tuple<string, string>> currentOutputList, byte[] source){
            int i = getIndex();
            string propertyName = " ";
            int senderSettle = getInt8(source.Skip(i).Take(1).ToArray());
            currentOutputList.Add(Tuple.Create("metadata", "1 byte wide int"));

            if(senderSettle == 0) {
                propertyName = "unsettled - The sender will send all deliveries initially unsettled to the receiver";
            } else if (senderSettle == 1) {
                propertyName = "settled - The sender will send all deliveries settled to the receiver";
            } else if (senderSettle == 2) {
                propertyName = "mixed - The sender MAY send a mixture of settled and unsettled deliveries to the receiver";
            }
            return propertyName;
        }
        public string getRecieverSettleMode(List<Tuple<string, string>> currentOutputList, byte[] source){
            int i = getIndex();
            string propertyName = " ";
            int senderSettle = getInt8(source.Skip(i).Take(1).ToArray());
            currentOutputList.Add(Tuple.Create("metadata", "1 byte wide int"));

            if(senderSettle == 0) {
                propertyName = "first - The receiver will spontaneously settle all incoming transfers";
            } else if (senderSettle == 1) {
                propertyName = "second - The receiver will only settle after sending the disposition to the sender and receiving a disposition indicating settlement of the delivery from the sender.";
            }
            return propertyName;
        }            
        
        public List<Tuple<string, string>> amqpTranslate(byte[] source) {
            List<Tuple<string, string>> currentOutputList = new List<Tuple<string, string>>();

            int i = 0;
            int lengthOfFrame = getInt32(source);
            i += 4;
            if (lengthOfFrame != source.Length) {
                currentOutputList.Add(Tuple.Create("data", "Frame length does not match provided frame"));
                return currentOutputList;
            } else {
                currentOutputList.Add(Tuple.Create("data", "Frame Length: "+ lengthOfFrame +" bytes "));
                int dOff = 4 * source[i]; 
                currentOutputList.Add(Tuple.Create("data", "Data offset at position: " + dOff));
                i++;
                
                if (source[i] == 0) {
                    currentOutputList.Add(Tuple.Create("data","Frame type: AMQP"));
                    i++;
                } else if (source[i]== 1) {
                    currentOutputList.Add(Tuple.Create("data", "Frame type: SASL"));
                    i++;
                } else if (source[i] != 1) {
                    currentOutputList.Add(Tuple.Create("data", "Type not found"));
                }

                int channel = getInt16(source.Skip(i).Take(2).ToArray());
                currentOutputList.Add(Tuple.Create("data", "Channel: " + channel));
                i += 2;

                if (dOff == i) {
                    currentOutputList.Add(Tuple.Create("data", "No extended header"));
                } else {
                    
                    currentOutputList.Add(Tuple.Create("data", "Extended header present: " + BitConverter.ToString(source.Skip(i).Take(dOff-i).ToArray())));
                    
                    i = dOff;
                }

                if (i == source.Length){
                    currentOutputList.Add(Tuple.Create("data", "No frame body present"));
                    return currentOutputList;
                }

                if (source[i]== 0) {
                    currentOutputList.Add(Tuple.Create("metadata","constructor start of 0x00"));
                    i++;
                }

                if(source[i] == uLong8) {
                    currentOutputList.Add(Tuple.Create("metadata", source[i].ToString("X2") + "  1 byte unsigned long type descriptor"));
                    i++;
                } else{
                    currentOutputList.Add(Tuple.Create("data", "Type descriptor not implemented"));
                }

                byte performative = source[i++];
                setIndex(i);

                switch (performative) {
                    case openPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Perfomative: OPEN"));
                        findList(currentOutputList, source);
                        findString(currentOutputList, source, "container id");
                        findString(currentOutputList, source, "hostname");
                        findUint(currentOutputList, source, "max frame");
                        findUshort(currentOutputList, source, "channel max");
                        findUint(currentOutputList, source, "idle timeout");
                        findSymbol(currentOutputList, source, "outgoing-locales ");
                        findSymbol(currentOutputList, source, "incomming-locales ");
                        findSymbol(currentOutputList, source, "offered Capabilities");
                        findSymbol(currentOutputList, source, "desired Capabilities");
                        findMap(currentOutputList, source, "properties");
                        break;
                    case beginPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: BEGIN"));
                        findList(currentOutputList, source);
                        findUshort(currentOutputList, source, "remote channel");
                        findUint(currentOutputList, source, "transfer number");
                        findUint(currentOutputList, source, "incoming window");
                        findUint(currentOutputList, source, "outgoing window");
                        findUint(currentOutputList, source, "handle max");
                        findSymbol(currentOutputList, source, "offered Capabilities");
                        findSymbol(currentOutputList, source, "desired Capabilities");                        
                        findMap(currentOutputList, source, "properties");
                        break;
                    case attachPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: ATTACH"));
                        findList(currentOutputList, source);
                        findString(currentOutputList, source, "name");
                        findUint(currentOutputList, source, "handle");
                        findBool(currentOutputList, source, "role");
                        findUbyte(currentOutputList, source, getSenderSettleMode(currentOutputList, source));
                        findUbyte(currentOutputList, source, getRecieverSettleMode(currentOutputList, source));

                        break;
                    case flowPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: FLOW"));
                        break;
                    case transferPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: TRANSFER"));
                        break;              
                    case dispositionPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: DISPOSITION"));
                        break;                    
                    case detachPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: DETACH"));
                        break;
                    case endPerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: END"));
                        break;
                    case closePerformative:
                        currentOutputList.Add(Tuple.Create("data", "Performative: CLOSE"));
                        break;
                    default:
                        currentOutputList.Add(Tuple.Create("data", "Performative not yet implemented"));
                        return currentOutputList;
                }

                return currentOutputList;
            }
        }
    }
}