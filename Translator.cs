using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace translator{
    public class Translator{
        public void addToArray(ref byte destination, byte source, int index){
                if (index % 2 == 0){
                    source = (byte)(source << 4);
                }
                destination |= source;  
            }

            public int getInt32(byte[] source){
                byte[] destination = new Byte[4];
                Array.Copy(source, destination, 4);
                Array.Reverse(destination);
                return BitConverter.ToInt32(destination, 0);
            }

            public int getInt16(byte[] source){
                byte[] destination = new Byte[2];
                Array.Copy(source, destination, 2);
                Array.Reverse(destination);
                return BitConverter.ToInt16(destination, 0);
            }            
            public List<string> amqpTranslate(byte[] source){
                List<string> currentOutputList = new List<string>(new string[] {" "});
                int i = 0;
                int lengthOfFrame = getInt32(source);
                i = i+4;
                if(lengthOfFrame != source.Length){
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
                        currentOutputList.Add("Extended header present");
                        i = dOff;
                    }
                    
                    return currentOutputList;
                }
            }

           
    }
}