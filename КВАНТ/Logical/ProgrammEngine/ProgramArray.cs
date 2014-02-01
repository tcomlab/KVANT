using System;
using System.Collections.Generic;
using КВАНТ.Logical.ProgrammEngine.array_l_element;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.Logical.ProgrammEngine
{
    [Serializable]
    public class ProgramArray
    {
        private object[,] _element;
        private int _x;
        private int _y;

        public int X { get { return _x; } }

        public int Y { get { return _y; } }

        public ProgramArray(int x,int y)
        {
            _x = x;
            _y = y;
            _element = new object[x,y];
            InitArray();
        }

        public ProgramArray()
        {
            _x = 2;
            _y = 2;
            _element = new object[_x, _y];
            InitArray();
        }

        private void InitArray()
        {
            for (int x = 0; x < _x; x++)
            {
                for (int y = 0; y < _y; y++)
                {
                    _element[x, y] = new Dummy();
                }
            }
        }

        public object[,] Element
        {
            set { _element = value; }
            get { return _element; }
        }

        public List<object> GetListOfType(КВАНТ.Logical.ProgrammEngine.array_l_element.Type t)
        {
            var eleList = new List<object>();
            for (int x = 0; x < _x; x++)
            {
                for (int y = 0; y < _y; y++)
                {
                    var el = (ILogicEngine) _element[x, y];
                    if (el.Type == t)
                    {
                        eleList.Add(_element[x, y]);
                    }
                }
            }
            return eleList;
        }

        public ElementData[] save_model()
        {
            int pointer = 0;
            var data = new ElementData[_x * _y];
            for (int x = 0; x < _x; x++)
            {
                for (int y = 0; y < _y; y++)
                {
                    data[pointer++] = ((IContentData)_element[x, y]).Content;
                }
            }
            return data;
        }

        public void load_model(ElementData[] data, int x, int y)
        {
            _x = x;
            _y = y;
            _element = new object[x, y];
            InitArray();
            int pointer = 0;
            for (int x1 = 0; x1 < _x; x1++)
            {
                for (int y1 = 0; y1 < _y; y1++)
                {
                    var element = data[pointer++];

                    switch (element.Type)
                    {
                        case Type.DI:
                            _element[x1, y1] = new DI(element);   
                            break;
                        case Type.DO:
                            _element[x1, y1] = new DO(element);   
                            break;
                        case Type.EN:
                            _element[x1, y1] = new EN(element);   
                            break;
                        case Type.DT:
                            _element[x1, y1] = new DT(element);   
                            break;
                        case Type.AI:
                            _element[x1, y1] = new Ai(element);   
                            break;
                        case Type.BT:
                            _element[x1, y1] = new BT(element);   
                            break;
                        case Type.TI:
                            _element[x1, y1] = new TI(element);   
                            break;
                        case Type.RT:
                            _element[x1, y1] = new RT(element);   
                            break;
                        case Type.II:
                            _element[x1, y1] = new Ii(element);   
                            break;
                        case Type.NN:
                            _element[x1, y1] = new Dummy();   
                            break;
                        case Type.MR:
                            _element[x1, y1] = new MR(element);   
                            break;
                        case Type.MW:
                            _element[x1, y1] = new MW(element);   
                            break;
                        case Type.KG:
                            _element[x1, y1] = new KG(element);   
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    

                }
            }
        }
    }
}
