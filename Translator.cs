using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace translator{
    public class Translator{

        private const byte list0 = 0x45;
        private const byte list8 = 0xc0;
        private const byte list32 = 0xd0;
        private const byte map8 = 0xc1;
        private const byte map32 = 0xd1;
        private const byte nullValue = 0x40;
        private const byte uint32 = 0x70;
        private const byte uint8 = 0x52;
        private const byte uint0 = 0x43;
        private const byte str8Utf = 0xa1;
        private const byte str32Utf = 0xb1;
        private const byte ushort16 = 0x60;
        private const byte sym8 = 0xa3;
        private const byte sym32 = 0xb3;
        private const byte ulong8 = 0x53;
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
        public int getInt32(byte[] source){
            byte[] destination = new Byte[4];
            Array.Copy(source, destination, 4);
            Array.Reverse(destination);
            return BitConverter.ToInt32(destination, 0);
        }

        public int getInt16(byte[] source) {
            byte[] destination = new Byte[2];
            Array.Copy(source, destination, 2);
            Array.Reverse(destination);
            return BitConverter.ToInt16(destination, 0);
        }
        public void translateOpenFrame(List<string> currentOutputList, byte[] source) {
            int i = 0;
            int numElementsInList = 0;
            byte listType = source[i];
            currentOutputList.Add("(Metadata) List Type: " + listType.ToString("X2"));
            i++;

            switch (listType){
                case list0:
                    currentOutputList.Add("List 0: no elements in open list");
                    numElementsInList = 0;
                    i++;
                    break;
                case list8:
                    currentOutputList.Add("(Metadata) List 8, Number bytes wide: " + source[i]);
                    i++;
                    numElementsInList = source[i];
                    currentOutputList.Add("There are " + numElementsInList + " elements in the list" );
                    i++;
                    break;
                case list32:
                    int numBytesInList = getInt32(source.Skip(1).Take(4).ToArray());
                    i = i + 4;
                    currentOutputList.Add("List 32, Number of bytes wide: " + numBytesInList);
                    numElementsInList = source[i];
                    currentOutputList.Add("There are " + numElementsInList + " in the list" );
                    i++;
                    break;
                default:
                    currentOutputList.Add("Invalid Open Frame");
                    break;
            }

            if(source[i] == str8Utf) {
                i++;
                int containerIdLength = source[i];
                i++;
                byte[] containerId = source.Skip(i).Take(containerIdLength).ToArray();
                i = i + containerIdLength;      
                currentOutputList.Add("(Metadata): String descriptor length can fit in 1 byte. Length is : " + containerIdLength);
                currentOutputList.Add("containerId: " + System.Text.Encoding.UTF8.GetString(containerId, 0, containerIdLength));
            } else if(source[i] == str32Utf) {
                i++;
                int containerIdLength = getInt32(source.Skip(1).Take(4).ToArray());
                i = i + 4;  
                byte[] containerId = source.Skip(i).Take(containerIdLength).ToArray();
                i = i + containerIdLength;      
                currentOutputList.Add("(Metadata): String descriptor length can fit into 4 bytes. Length is : " + containerIdLength);
                currentOutputList.Add("containerId: " + System.Text.Encoding.UTF8.GetString(containerId, 0, containerIdLength));
            }
            
            if (source[i] == nullValue){
                i++;
                currentOutputList.Add("Hostname not present");

            } else if (source[i] == str8Utf) {
                i++;
                int hostnameLength = source[i];
                i++;
                byte[] hostname = source.Skip(i).Take(hostnameLength).ToArray();
                currentOutputList.Add("(Metadata): String descriptor length can fit into 4 bytes. Length is : " + hostnameLength);                
                currentOutputList.Add("Hostname: " + System.Text.Encoding.UTF8.GetString(hostname, 0, hostnameLength));
                i = i + hostnameLength;
            } else if (source[i] == str32Utf){
                 i++;
                int hostnameLength = getInt32(source.Skip(1).Take(4).ToArray());
                i = i + 4;  
                byte[] hostname = source.Skip(i).Take(hostnameLength).ToArray();
                i = i + hostnameLength;
                currentOutputList.Add("(Metadata): String descriptor length can fit into 4 bytes. Length is : " + hostnameLength);
                currentOutputList.Add("containerId: " + System.Text.Encoding.UTF8.GetString(hostname, 0, hostnameLength));
            }

            if(source[i] == uint32){
                i++;
                currentOutputList.Add("Metadata: 4 byte wide frame");
                currentOutputList.Add("Max Frame Size: " + getUInt32(source.Skip(i).Take(4).ToArray()));
                i = i + 4;

            } else if (source[i] == uint8) {
                i++;                
                currentOutputList.Add("(Metadata: byte wide frame) Max Frame Size: " + getUInt16(source.Skip(i).Take(2).ToArray()));
                i = i + 2;

            } else if (source[i] == uint0) {
                i++;                
                currentOutputList.Add("(Metadata: 0 byte wide frame) Max Frame Size: 0");
            }

            if (source[i] == ushort16){
                i++;
                currentOutputList.Add("16-bit unsigned integer in network byte order: " + getUInt16(source.Skip(i).Take(2).ToArray()));
                i += 2;
            } else if (source[i] == nullValue) {
                i++;
                currentOutputList.Add("Chanel max not present, using  default");
            }

            if (source[i] == uint32) {
                i++;
                currentOutputList.Add("(metadata) 32-bit unsigned integer");
                currentOutputList.Add("Idle Timeout: " + getUInt32(source.Skip(i).Take(4).ToArray()) + " milliseconds");
                i = i + 4;

            } else if (source[i] == uint8) {
                i++;
                currentOutputList.Add("(metadata) 16-bit unsigned integer");
                currentOutputList.Add("Idle Timeout: " + getUInt16(source.Skip(i).Take(2).ToArray()) + " milliseconds");
                i += 2;

            } else if (source[i] == uint0) {
                i++;
                currentOutputList.Add("(metadata) 0-bit unsigned integer");
                currentOutputList.Add("Idle Timeout: 0 milliseconds");
            }

            if (source[i] == sym8){
                i++;
                currentOutputList.Add("(Metadata) symbol with 1-byte length");
                int numCharInSymbol = source[i];
                i++; 
                currentOutputList.Add("Outgoing-locales: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;

            } else if (source[i] == sym32){
                i++;
                currentOutputList.Add("(Metadata) symbol with 4-byte length");
                int numCharInSymbol = getInt32(source.Skip(i).Take(4).ToArray());                
                i += 4;
                currentOutputList.Add("Outgoing-locales: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;
            }
            
            if (source[i] == sym8){
                i++;
                currentOutputList.Add("(Metadata) symbol with 1-byte length");
                int numCharInSymbol = source[i];
                i++; 
                currentOutputList.Add("Incoming-locales: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;

            } else if (source[i] == sym32){
                i++;
                currentOutputList.Add("(Metadata) symbol with 4-byte length");
                int numCharInSymbol = getInt32(source.Skip(i).Take(4).ToArray());                
                i += 4;
                currentOutputList.Add("Incoming-locales: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;
            }

            if (source[i] == nullValue) {
                i++;
                currentOutputList.Add("Offered Capabilities not present");
            } else if (source[i] == sym8){
                i++;
                currentOutputList.Add("(Metadata) symbol with 1-byte length");
                int numCharInSymbol = source[i];
                i++; 
                currentOutputList.Add("Offered Capabilities: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;

            } else if (source[i] == sym32){
                i++;
                currentOutputList.Add("(Metadata) symbol with 4-byte length");
                int numCharInSymbol = getInt32(source.Skip(i).Take(4).ToArray());                
                i += 4;
                currentOutputList.Add("Offered Capabilities: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;
            }

            if (source[i] == nullValue) {
                i++;
                currentOutputList.Add("Desired Capabilities not present");
            } else if (source[i] == sym8){
                i++;
                currentOutputList.Add("(Metadata) symbol with 1-byte length");
                int numCharInSymbol = source[i];
                i++; 
                currentOutputList.Add("Desired Capabilities: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;

            } else if (source[i] == sym32) {
                i++;
                currentOutputList.Add("(Metadata) symbol with 4-byte length");
                int numCharInSymbol = getInt32(source.Skip(i).Take(4).ToArray());                
                i += 4;
                currentOutputList.Add("Desired Capabilities: " + System.Text.Encoding.UTF8.GetString(source, i, numCharInSymbol));
                i += numCharInSymbol;
            }

            if (source[i] == nullValue){
                i++;
                currentOutputList.Add("Properties not present");
            } else if (source[i] == map8) {
                i++;
                var mapLength = source[i];
                currentOutputList.Add("(Metadata) 1 byte length,  " + mapLength + " bytes long");
                i++;
                currentOutputList.Add("Properties: " + BitConverter.ToString(source.Skip(i).Take(mapLength).ToArray()));
                i += mapLength;
            } else if (source[i] == map32) {
                i++;
                var mapLength = getInt32(source.Skip(i).Take(4).ToArray());
                currentOutputList.Add("(Metadata) Map is 1 byte length,  " + mapLength + " bytes long");
                i += 4;
                currentOutputList.Add("Properties: " + BitConverter.ToString(source.Skip(i).Take(mapLength).ToArray()));
                i += mapLength;
            }
        }
        public List<string> amqpTranslate(byte[] source) {
            List<string> currentOutputList = new List<string>(new string[] {" "});
            int i = 0;
            int lengthOfFrame = getInt32(source);
            i = i + 4;
            if (lengthOfFrame != source.Length) {
                currentOutputList.Add("Frame length does not match provided frame");
                return currentOutputList;
            } else {
                currentOutputList.Add("Frame Length: "+ lengthOfFrame +" bytes ");
                int dOff = 4 * source[i]; 
                currentOutputList.Add("Data offset at position: " + dOff);
                i++;
                
                if (source[i] == 0) {
                    currentOutputList.Add("Frame type: AMQP");
                    i++;
                } else if (source[i]== 1) {
                    currentOutputList.Add("Frame type: SASL");
                    i++;
                } else if (source[i] != 1) {
                    currentOutputList.Add("Type not found");
                }

                int channel = getInt16(source.Skip(i).Take(2).ToArray());
                currentOutputList.Add("Channel: " + channel);
                i = i + 2;

                if (dOff == i) {
                    currentOutputList.Add("No extended header");
                } else {
                    
                    currentOutputList.Add("Extended header present: " + BitConverter.ToString(source.Skip(i).Take(dOff-i).ToArray()));
                    
                    i = dOff;
                }

                if (i == source.Length){
                    currentOutputList.Add("No frame body present");
                    return currentOutputList;
                }

                if (source[i]== 0) {
                    currentOutputList.Add("Metadata: constructor start of 0x00");
                    i++;
                }

                if(source[i] == ulong8) {
                    currentOutputList.Add("Metadata:  " + source[i].ToString("X2") + "  1 byte unsigned long type descriptor");
                    i++;
                } else{
                    currentOutputList.Add("Type descriptor not implemented");
                }

                byte performative = source[i++];
                
                switch (performative) {
                    case openPerformative:
                        currentOutputList.Add("Perfomative: OPEN");
                        translateOpenFrame(currentOutputList, source.Skip(i).Take(lengthOfFrame - 1).ToArray());
                        break;
                    case beginPerformative:
                        currentOutputList.Add("Performative: BEGIN");
                        break;
                    case attachPerformative:
                        currentOutputList.Add("Performative: ATTACH");
                        break;
                    case flowPerformative:
                        currentOutputList.Add("Performative: FLOW");
                        break;
                    case transferPerformative:
                        currentOutputList.Add("Performative: TRANSFER");
                        break;              
                    case dispositionPerformative:
                        currentOutputList.Add("Performative: DISPOSITION");
                        break;                    
                    case detachPerformative:
                        currentOutputList.Add("Performative: DETACH");
                        break;
                    case endPerformative:
                        currentOutputList.Add("Performative: END");
                        break;
                    case closePerformative:
                        currentOutputList.Add("Performative: CLOSE");
                        break;
                    default:
                        currentOutputList.Add("Performative not yet implemented");
                        return currentOutputList;
                }

                return currentOutputList;
            }
        }
    }
}