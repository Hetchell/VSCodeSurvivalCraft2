
using Engine;
using Game;
using System;
using System.Collections.Generic;
using System.Text;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class EditMemoryBankDialogAPI : Dialog
    {
        public MemoryBankData memory;

        public DynamicArray<byte> Data = new DynamicArray<byte>();

        public StackPanelWidget MainView;

        public Action onCancel;

        public int clickpos;

        public bool isSetPos;

        public int setPosN;

        public int lastvalue;

        public bool isclick = true;

        public List<ClickTextWidget> list = new List<ClickTextWidget>();

        public byte LastOutput
        {
            get;
            set;
        }

        public EditMemoryBankDialogAPI(MemoryBankData memoryBankData, Action onCancel)
        {
            memory = memoryBankData;
            Data.Clear();
            Data.AddRange(memory.Data);
            CanvasWidget canvasWidget = new CanvasWidget
            {
                Size = new Vector2(600f, 500f),
                HorizontalAlignment = WidgetAlignment.Center,
                VerticalAlignment = WidgetAlignment.Center
            };
            RectangleWidget widget = new RectangleWidget
            {
                FillColor = new Color(0, 0, 0, 255),
                OutlineColor = new Color(128, 128, 128, 128),
                OutlineThickness = 2f
            };
            StackPanelWidget stackPanelWidget = new StackPanelWidget
            {
                Direction = LayoutDirection.Vertical
            };
            LabelWidget widget2 = new LabelWidget
            {
                Text = LanguageControl.GetContentWidgets(GetType().Name, 0),
                HorizontalAlignment = WidgetAlignment.Center,
                Margin = new Vector2(0f, 10f)
            };
            StackPanelWidget stackPanelWidget2 = new StackPanelWidget
            {
                Direction = LayoutDirection.Horizontal,
                HorizontalAlignment = WidgetAlignment.Near,
                VerticalAlignment = WidgetAlignment.Near,
                Margin = new Vector2(10f, 10f)
            };
            Children.Add(canvasWidget);
            canvasWidget.Children.Add(widget);
            canvasWidget.Children.Add(stackPanelWidget);
            stackPanelWidget.Children.Add(widget2);
            stackPanelWidget.Children.Add(stackPanelWidget2);
            stackPanelWidget2.Children.Add(initData());
            stackPanelWidget2.Children.Add(initButton());
            //q-in
            Led_Init();
            Led_Load(canvasWidget);
            MainView = stackPanelWidget;
            this.onCancel = onCancel;
            lastvalue = memory.Read(0);
        }

        public byte Read(int address)
        {
            if (address >= 0 && address < Data.Count)
            {
                return Data.Array[address];
            }

            return 0;
        }

        public void Write(int address, byte data)
        {
            if (address >= 0 && address < Data.Count)
            {
                Data.Array[address] = data;
            }
            else if (address >= 0 && address < 256 && data != 0)
            {
                Data.Count = MathUtils.Max(Data.Count, address + 1);
                Data.Array[address] = data;
            }
        }

        public void LoadString(string data)
        {
            string[] array = data.Split(new char[1]
            {
                ';'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length >= 1)
            {
                string text = array[0];
                text = text.TrimEnd('0');
                Data.Clear();
                for (int i = 0; i < MathUtils.Min(text.Length, 256); i++)
                {
                    int num = MemoryBankData.m_hexChars.IndexOf(char.ToUpperInvariant(text[i]));
                    if (num < 0)
                    {
                        num = 0;
                    }

                    Data.Add((byte)num);
                }
            }

            if (array.Length >= 2)
            {
                string text2 = array[1];
                int num2 = MemoryBankData.m_hexChars.IndexOf(char.ToUpperInvariant(text2[0]));
                if (num2 < 0)
                {
                    num2 = 0;
                }

                LastOutput = (byte)num2;
            }
        }

        public string SaveString()
        {
            return SaveString(saveLastOutput: true);
        }

        public string SaveString(bool saveLastOutput)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int count = Data.Count;
            for (int i = 0; i < count; i++)
            {
                int index = MathUtils.Clamp(Data.Array[i], 0, 15);
                stringBuilder.Append(MemoryBankData.m_hexChars[index]);
            }

            if (saveLastOutput)
            {
                stringBuilder.Append(';');
                stringBuilder.Append(MemoryBankData.m_hexChars[MathUtils.Clamp(LastOutput, 0, 15)]);
            }

            return stringBuilder.ToString();
        }

        public Widget initData()
        {
            StackPanelWidget stackPanelWidget = new StackPanelWidget
            {
                Direction = LayoutDirection.Vertical,
                VerticalAlignment = WidgetAlignment.Center,
                HorizontalAlignment = WidgetAlignment.Far,
                Margin = new Vector2(10f, 0f)
            };
            for (int i = 0; i < 17; i++)
            {
                StackPanelWidget stackPanelWidget2 = new StackPanelWidget
                {
                    Direction = LayoutDirection.Horizontal
                };
                for (int j = 0; j < 17; j++)
                {
                    int addr = (i - 1) * 16 + (j - 1);
                    if (j > 0 && i > 0)
                    {
                        ClickTextWidget clickTextWidget = new ClickTextWidget(new Vector2(22f), $"{MemoryBankData.m_hexChars[Read(addr)]}", delegate
                        {
                            AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                            clickpos = addr;
                            isclick = true;
                        });
                        list.Add(clickTextWidget);
                        stackPanelWidget2.Children.Add(clickTextWidget);
                        continue;
                    }

                    int num = 0;
                    if (i == 0 && j > 0)
                    {
                        num = j - 1;
                    }
                    else
                    {
                        if (j != 0 || i <= 0)
                        {
                            ClickTextWidget widget = new ClickTextWidget(new Vector2(22f), "", null);
                            stackPanelWidget2.Children.Add(widget);
                            continue;
                        }

                        num = i - 1;
                    }

                    ClickTextWidget clickTextWidget2 = new ClickTextWidget(new Vector2(22f), MemoryBankData.m_hexChars[num].ToString(), delegate
                    {
                    });
                    clickTextWidget2.labelWidget.Color = Color.DarkGray;
                    stackPanelWidget2.Children.Add(clickTextWidget2);
                }

                stackPanelWidget.Children.Add(stackPanelWidget2);
            }

            return stackPanelWidget;
        }

        //侧边按钮
        public Widget makeFuncButton(string txt, Action func)
        {
            return new ClickTextWidget(new Vector2(50f, 40f), txt, func, box: true)
            {
                BackGround = Color.White,
                Margin = new Vector2(5f, 2f),
                labelWidget =
                {
                    FontScale = 1f,
                    Color = Color.Black,
                    ColorTransform = Color.Yellow


                }
            };
        }

        public Widget initButton()
        {
            StackPanelWidget stackPanelWidget = new StackPanelWidget
            {
                Direction = LayoutDirection.Vertical,
                VerticalAlignment = WidgetAlignment.Center,
                HorizontalAlignment = WidgetAlignment.Far,
                Margin = new Vector2(10f, 10f)
            };
            for (int i = 0; i < 6; i++)
            {
                StackPanelWidget stackPanelWidget2 = new StackPanelWidget
                {
                    Direction = LayoutDirection.Horizontal
                };
                for (int j = 0; j < 3; j++)
                {
                    int num = i * 3 + j;
                    if (num < 15)
                    {
                        int pp = num + 1;
                        stackPanelWidget2.Children.Add(makeFuncButton($"{MemoryBankData.m_hexChars[pp]}", delegate
                        {
                            AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                            if (!isSetPos)
                            {
                                Write(clickpos, (byte)pp);
                                lastvalue = pp;
                                clickpos++;
                                //point220117A01
                                if (clickpos > 255)
                                {
                                    clickpos = 0;
                                }

                                isclick = true;
                            }
                            else
                            {
                                if (setPosN == 0)
                                {
                                    clickpos = 16 * pp;
                                }
                                else if (setPosN == 1)
                                {
                                    clickpos += pp;
                                }

                                setPosN++;
                                if (setPosN == 2)
                                {
                                    if (clickpos > 255)
                                    {
                                        clickpos = 0;
                                    }

                                    setPosN = 0;
                                    isclick = true;
                                    isSetPos = false;
                                }
                            }
                        }));
                        continue;
                    }

                    switch (num)
                    {
                        case 15:
                            stackPanelWidget2.Children.Add(makeFuncButton($"{MemoryBankData.m_hexChars[0]}", delegate
                            {
                                AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                                if (!isSetPos)
                                {
                                    Write(clickpos, 0);
                                    lastvalue = 0;
                                    clickpos++;
                                    //point220117A01
                                    if (clickpos > 255)
                                    {
                                        clickpos = 0;
                                    }

                                    isclick = true;
                                }
                                else
                                {
                                    if (setPosN == 0)
                                    {
                                        clickpos = 0;
                                    }
                                    else if (setPosN == 1)
                                    {
                                        //clickpos +=0;
                                    }

                                    setPosN++;
                                    if (setPosN == 2)
                                    {
                                        if (clickpos > 255)
                                        {
                                            clickpos = 0;
                                        }

                                        setPosN = 0;
                                        isclick = true;
                                        isSetPos = false;
                                    }
                                }
                            }));
                            break;
                        case 16:

                            stackPanelWidget2.Children.Add(makeFuncButton("Clr", delegate
                            {
                                AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                                for (int l = 0; l < Data.Count; l++)
                                {
                                    Write(l, 0);
                                }

                                isclick = true;
                            }));
                            break;
                        case 17:
                            stackPanelWidget2.Children.Add(makeFuncButton("Trl", delegate
                            {
                                AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                                DynamicArray<byte> dynamicArray = new DynamicArray<byte>();
                                dynamicArray.AddRange(Data);
                                dynamicArray.Count = 256;
                                for (int m = 0; m < 16; m++)
                                {
                                    for (int n = 0; n < 16; n++)
                                    {
                                        Write(m + n * 16, dynamicArray[m * 16 + n]);
                                    }
                                }

                                clickpos = 0;
                                isclick = true;
                            }));
                            break;
                    }
                }

                stackPanelWidget.Children.Add(stackPanelWidget2);
            }

            LabelWidget widget = new LabelWidget
            {
                FontScale = 0.8f,
                Text = LanguageControl.GetContentWidgets(GetType().Name, 3),
                HorizontalAlignment = WidgetAlignment.Center,
                Margin = new Vector2(0f, 10f),
                Color = Color.DarkGray
            };
            stackPanelWidget.Children.Add(widget);
            stackPanelWidget.Children.Add(makeTextBox(delegate (TextBoxWidget textBoxWidget)
            {
                LoadString(textBoxWidget.Text);
                isclick = true;
            }, memory.SaveString(saveLastOutput: false)));
            stackPanelWidget.Children.Add(makeButton("OK", delegate
            {
                for (int k = 0; k < Data.Count; k++)
                {
                    memory.Write(k, Data[k]);
                }

                onCancel?.Invoke();
                AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                DialogsManager.HideDialog(this);
            }));
            stackPanelWidget.Children.Add(makeButton("Cancel", delegate
            {
                AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0f, 0f);
                DialogsManager.HideDialog(this);
                isclick = true;
            }));
            return stackPanelWidget;
        }

        public Widget makeTextBox(Action<TextBoxWidget> ac, string text = "")
        {
            CanvasWidget obj = new CanvasWidget
            {
                HorizontalAlignment = WidgetAlignment.Center
            };
            //text条
            RectangleWidget widget = new RectangleWidget
            {
                FillColor = Color.Black,
                OutlineColor = Color.White,
                Size = new Vector2(120f, 30f)
            };
            StackPanelWidget stackPanelWidget = new StackPanelWidget
            {
                Direction = LayoutDirection.Vertical
            };
            TextBoxWidget textBoxWidget = new TextBoxWidget
            {
                VerticalAlignment = WidgetAlignment.Center,
                Color = new Color(255, 255, 255),
                Margin = new Vector2(4f, 0f),
                Size = new Vector2(120f, 30f),
                MaximumLength = 256
            };
            textBoxWidget.FontScale = 0.7f;
            textBoxWidget.Text = text;
            textBoxWidget.TextChanged += ac;
            stackPanelWidget.Children.Add(textBoxWidget);
            obj.Children.Add(widget);
            obj.Children.Add(stackPanelWidget);
            return obj;
        }

        public Widget makeButton(string txt, Action tas)
        {
            return new ClickTextWidget(new Vector2(120f, 30f), txt, tas)
            {
                /*rectangleWidget = new RectangleWidget
                {
                    FillColor = Color.Gray,
                    OutlineColor = Color.Transparent,
                    OutlineThickness = 1f,
                },*/
                BackGround = Color.Gray,

                /*ColorTransform = Color.White,
                BackGround = Color.White,*/
                Margin = new Vector2(0f, 3f),
                labelWidget =
                {
                   
                    //FontScale = 0.7f,
                    Color = Color.Green

                }
            };
        }

        public override void Update()
        {
            if (base.Input.Back || base.Input.Cancel)
            {
                DialogsManager.HideDialog(this);
            }

            if (isSetPos)
            {
                list[clickpos].BackGround = Color.Green;
            }
            else
            {
                if (!isclick)
                {
                    return;
                }
                //point
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == clickpos)
                    {
                        list[i].ColorTransform = Color.Yellow;
                        //q-in
                        Led_SetColor(i);
                    }
                    else
                    {
                        list[i].ColorTransform = Color.White;
                        list[i].BackGround = Color.Transparent;

                    }

                    list[i].labelWidget.Text = $"{MemoryBankData.m_hexChars[Read(i)]}";
                }

                isclick = false;
            }
        }
        private readonly RectangleWidget[] Led_light = new RectangleWidget[4];
        private void Led_Init()
        {
            int temp = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Led_light[temp] = new RectangleWidget
                    {
                        Size = new Vector2(15, 15),
                        Margin = new Vector2(j * 15, i * 15),
                        OutlineColor = Color.Violet,
                        FillColor = Color.White,
                        IsVisible = true
                    };
                    temp++;
                }
            }

        }
        private void Led_Load(CanvasWidget canvasWidget)
        {
            canvasWidget.Children.Add(Led_light);
        }
        private void Led_SetColor(int IsLight_Value)
        {
            Led_light[0].FillColor = ((Read(IsLight_Value) & 1) >> 0 == 1) ? Color.White : Color.Black;
            Led_light[1].FillColor = ((Read(IsLight_Value) & 2) >> 1 == 1) ? Color.White : Color.Black;
            Led_light[2].FillColor = ((Read(IsLight_Value) & 4) >> 2 == 1) ? Color.White : Color.Black;
            Led_light[3].FillColor = ((Read(IsLight_Value) & 8) >> 3 == 1) ? Color.White : Color.Black;
        }
    }
}