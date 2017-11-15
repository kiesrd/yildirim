/******************************************************************************/
/**
  * ###################################################
  * ### ###     KIES RESEARCH AND DEVELOPMENT   ### ###
  * ### ###             kiesrd.com              ### ###
  * ### ###         enginsubasi@gmail.com       ### ###
  * ### ###         YILDIRIM DRIVER V1          ### ###
  * ### ###             16/04/2016              ### ###
  * ###################################################
  *
  * ### Class Description 
  * ### This class to control Yildirim Digital v1 with .NET platform
  *
  * ###### Function Description
  *
  * ### bool start();
  * # Brief: Starts communication with Yildirim
  * # retval: Is active info
  *
  * ### bool stop();
  * # Brief: Stops communication with Yildirim
  * # retval: Is active info
  *
  * ### bool isActive();
  * # retval: Is active info
  *
  * ### void setPortName(string name);
  * # Brief: name input is Yildirim Com port name like "COM1", "COM13" etc.
  *
  * ### void setPinDir(char pin, int dir);
  * # Brief: Set pin direction
  * # Input Pin: Pin id like '0', '6', 'B' etc.
  * # Input Dir: Pin direction. 0: Input, 1: Output 
  * 
  * ### void setPinVal(char pin, int val);
  * # Input Pin: Pin id like '2', '3', 'F' etc.
  * # Input Val: Pin Value. 0: Low, 1: High 
  *
  * ### char readPinVal(char pin)
  * # Input Pin: Pin id like '4', '9', 'C' etc.
  * # retval: Pin Value. 0: Low, 1: High e: Crc Error
  *
  */

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace YILDIRIM_Demo
{
    class YildirimDriver
    {
        static SerialPort comPortY;

        public YildirimDriver()
        {
            comPortY = new SerialPort();
            
            comPortY.BaudRate = 115200;
            comPortY.StopBits = StopBits.One;
            comPortY.Parity = Parity.None;
            comPortY.ReadTimeout = 1000;
        }

        public YildirimDriver(string portName)
        {
            comPortY =  new SerialPort();

            comPortY.PortName   = portName;
            comPortY.BaudRate   = 115200;
            comPortY.StopBits   = StopBits.One;
            comPortY.Parity     = Parity.None;
            comPortY.ReadTimeout = 1000;
        }

        public bool start()
        {
            if( !comPortY.IsOpen )
                comPortY.Open();
            return (comPortY.IsOpen);
        }

        public bool stop()
        {
            comPortY.Close();
            return (comPortY.IsOpen);
        }

        public bool isActive()
        {
            return (comPortY.IsOpen);
        }

        public void setPortName(string name)
        {
            comPortY.PortName = name;
        }

        public byte calcCrc(byte[] inp)
        {
            byte crcInit = 0x55;
            int size = inp.Length - 2;

            for ( int i = 1; i < size; ++i)
            {
                crcInit ^= inp[i];
            }

            if( crcInit == 0x00 || crcInit == 0x02 || crcInit == 0x03 )
            {
                crcInit = 0x01;
            }
            else { }

            return (crcInit);
        }

        public string getYildirimID()
        {
            UInt32 yildirimID = new UInt32();

            if (isActive())
            {
                comPortY.Write("T"); // This string include some invisible chars

                string tmp;

                char x = Convert.ToChar( comPortY.ReadByte() );

                tmp = x.ToString();

                do
                {
                    x = Convert.ToChar(comPortY.ReadByte());
                    tmp += x.ToString();
                } while (x != 0x03);

                return (tmp);
            }
            else
            {
                yildirimID = 0;
            }

            return (yildirimID.ToString());
        }

        public void setPinDir(int pin, int dir)
        {
            List<byte> lst = new List<byte>();

            lst.Add(0x02);
            lst.Add(0x10);
            lst.Add(Convert.ToByte(pinToChar(pin)));
            lst.Add(Convert.ToByte(dir+48));
            lst.Add(0x01);
            lst.Add(0x03);

            lst[lst.Count - 2] = calcCrc(lst.ToArray());
            
            comPortY.Write(lst.ToArray(), 0, lst.Count);
        }

        public void setPinVal(int pin, int val)
        {
            List<byte> lst = new List<byte>();

            lst.Add(0x02);
            lst.Add(0x11);
            lst.Add(Convert.ToByte(pinToChar(pin)));
            lst.Add(Convert.ToByte(val + 48));
            lst.Add(0x01);
            lst.Add(0x03);

            lst[lst.Count - 2] = calcCrc(lst.ToArray());

            comPortY.Write(lst.ToArray(), 0, lst.Count);
        }

        public char readPinVal(int pin)
        {
            List<byte> lst = new List<byte>();
            List<byte> lstRec = new List<byte>();

            lst.Add(0x02);
            lst.Add(0x12);
            lst.Add(Convert.ToByte(pinToChar(pin)));
            lst.Add(0x01);
            lst.Add(0x03);

            lst[lst.Count - 2] = calcCrc(lst.ToArray());

            comPortY.Write(lst.ToArray(), 0, lst.Count);

            string tmp;
            
            char x = Convert.ToChar(comPortY.ReadByte());
            tmp = x.ToString();

            do
            {
                x = Convert.ToChar(comPortY.ReadByte());
                tmp += x.ToString();
            } while (x!=0x03 );

            

            for ( int i = 0; i < tmp.Length; ++i)
            {
                lstRec.Add(Convert.ToByte(tmp.ToCharArray()[i]));
            }
            
            if ( calcCrc(lstRec.ToArray()) == lstRec[lst.Count-2] )
            {
                return (Convert.ToChar(tmp.ToCharArray()[2]));
            }
            else
            {
                return ('e');
            }
        }

        public UInt16 readAnPinVal(int pin)
        {
            List<byte> lst = new List<byte>();
            List<byte> lstRec = new List<byte>();
            List<UInt16> lstTemp = new List<UInt16>();

            lst.Add(0x02);
            lst.Add(0x21);
            lst.Add(Convert.ToByte(pinToChar(pin)));
            lst.Add(0x01);
            lst.Add(0x03);

            lst[lst.Count - 2] = calcCrc(lst.ToArray());

            comPortY.ReadExisting();
            comPortY.Write(lst.ToArray(), 0, lst.Count);
            string tmp;
            char x = Convert.ToChar(comPortY.ReadByte());
            tmp = x.ToString();

            do
            {
                x = Convert.ToChar(comPortY.ReadByte());
                tmp += x.ToString();
            }while (x != 0x03);
            
            for (int i = 0; i < tmp.Length; ++i)
            {
                lstRec.Add(Convert.ToByte(tmp.ToCharArray()[i]));
            }

            if (calcCrc(lstRec.ToArray()) == lstRec[lstRec.Count - 2])
            {
                int x_ = (Convert.ToUInt16(tmp[2]) - 48) * 1000 +
                         (Convert.ToUInt16(tmp[3]) - 48) * 100 +
                         (Convert.ToUInt16(tmp[4]) - 48) * 10 +
                         (Convert.ToUInt16(tmp[5]) - 48) ;
                return ((UInt16)x_);
            }
            else
            {
                return('e');
            }
        }

        public UInt16 readBgAnPinVal(int pin)
        {
            List<byte> lst = new List<byte>();
            List<byte> lstRec = new List<byte>();
            List<UInt16> lstTemp = new List<UInt16>();

            lst.Add(0x02);
            lst.Add(0x22);
            lst.Add(Convert.ToByte(pinToChar(pin)));
            lst.Add(0x01);
            lst.Add(0x03);

            lst[lst.Count - 2] = calcCrc(lst.ToArray());

            comPortY.ReadExisting();
            comPortY.Write(lst.ToArray(), 0, lst.Count);
            string tmp;
            char x = Convert.ToChar(comPortY.ReadByte());
            tmp = x.ToString();

            do
            {
                x = Convert.ToChar(comPortY.ReadByte());
                tmp += x.ToString();
            } while (x != 0x03);

            for (int i = 0; i < tmp.Length; ++i)
            {
                lstRec.Add(Convert.ToByte(tmp.ToCharArray()[i]));
            }

            if (calcCrc(lstRec.ToArray()) == lstRec[lstRec.Count - 2])
            {
                int x_ = (Convert.ToUInt16(tmp[2]) - 48) * 1000 +
                         (Convert.ToUInt16(tmp[3]) - 48) * 100 +
                         (Convert.ToUInt16(tmp[4]) - 48) * 10 +
                         (Convert.ToUInt16(tmp[5]) - 48);
                return ((UInt16)x_);
            }
            else
            {
                return ('e');
            }
        }

        char pinToChar(int pin)
        {
            if ( pin == 0 )
            {
                return ('0');
            }
            else if (pin == 1)
            {
                return ('1');
            }
            else if (pin == 2)
            {
                return ('2');
            }
            else if (pin == 3)
            {
                return ('3');
            }
            else if (pin == 4)
            {
                return ('4');
            }
            else if (pin == 5)
            {
                return ('5');
            }
            else if (pin == 6)
            {
                return ('6');
            }
            else if (pin == 7)
            {
                return ('7');
            }
            else if (pin == 8)
            {
                return ('8');
            }
            else if (pin == 9)
            {
                return ('9');
            }
            else if (pin == 10)
            {
                return ('A');
            }
            else if (pin == 11)
            {
                return ('B');
            }
            else if (pin == 12)
            {
                return ('C');
            }
            else if (pin == 13)
            {
                return ('D');
            }
            else if (pin == 14)
            {
                return ('E');
            }
            else if (pin == 15)
            {
                return ('F');
            }
            else if (pin == 16)
            {
                return ('G');
            }
            else if (pin == 17)
            {
                return ('H');
            }
            else if (pin == 18)
            {
                return ('I');
            }
            else if (pin == 19)
            {
                return ('J');
            }
            else
            {
                return ('A');
            }
        }
    }
}
