# ImageSplitter (рабочее название)
#### История проекта
Данный проект был создан как временное решение. Я давно хотел создать приложение для управления локальными коллекциями изображений, при помощи тегов. Однако, этот проект постоянно стопорился, и я даже несколько раз начинал его с ноля, но постоянно забрасывал - то времени не хватало, то интереснее было разрабатывать другие вещи. При всём этом, необходимость упорядочивания больших коллекций изображений никуда не делась. Изначально я делал это всё вручную - открывал 5-7 папок одновременно, и из основного окна копировал файлы в эти папки. Промучавшись таким образом несколько месяцев (правда, упорядочивал новые поступления в коллекцию за это время я всего 2-3 раза, но убивал на это по 3-4 часа), я решил как-то это автоматизировать. Так и появилась самая первая версия данной программы.  

Первая версия была довольно примитивной - я указывал в ней 2 пути к папкам, а она в ответ по первому из них искала файлы изображений, а по второму - папки для раскладывания. После поиска, все файлы отображались в основном окне, и можно было стрелками (влево/вправо) листать картинки, а кнопками нампада перемещать их в целевые папки (список выводится в правой части окна). Собственно, в этой части функционала изменилось не многое. 

Дальше, я немного развил функционал программы. Я ещё лет 10 назад писал небольшую утилитку, которой пользовался и до сих пор. В неё передаётся путь к папке и количество файлов, которое должно быть в дочерних папках. А она в ответ, получает все файлы из целевой папки, и создаёт в ней дочерние папки, раскладывая найденные файлы в них, в указанном количестве на папку. Собственно, чтобы не запускать её при необходимости разбить файлы, я этот функционал встроил в эту программу. И в последствии дополнил её обратным - программа по переданному пути сканирует все дочерние папки, и все файлы из них переносит в родительскую. Затем, была и вовсе небольшая доработка, с добавлением функционала переименования файлов по маске. Этот функционал есть во многих готовых решениях, но я хотел иметь его под рукой, поэтому и добавил в программу.

#### Текущее состояние проекта
Буквально на днях, я решил что буду развивать именно этот проект, а тегированный просмотр отложу в совсем долгий ящик. Одной из главных сложностей тут является то, что проект писался буквально "на коленке", с тем рассчётом, что я попользуюсь им 2-3 раза а дальше заменю уже работой с тегами. Соответственно, я ни с красотой кода ни со структурой проекта особо не заморачивался - лишь бы работало. Но - вышло как вышло, и в ходе дальнейших доработок мне пришлось убить достаточно времени на то чтобы сделать этот код более-менее приличным. Но - интерфейс пока что оставляет желать лучшего, как с точки зрения визуала, так и с точки зрения понятности. Зато, я за последние несколько дней сильно расширил функционал.

Во первых, мне понадобился функционал раскладки изображений в заранее неизвестное количество папок, с именами, которые желательно указывать вручную. Таким образом, панелька на которой я просто выводил названия папок и проассоциированных к ним клавишь, была заменена на более функциональную - на ней уже можно было прямо во время работы программы добавлять или удалять папки. При добавлении появлялось окошко для ввода имени, с заранее сгенерированным именем, формата "New folder ({n})". Плюс - всякие проверки на существование папки с таким именем и т.п. Плюс - я доработал ассоциирование папок с кнопками, и теперь кнопки, которые не получится обработать (например - F13-F24) не будут выдаваться. Плюс - при удалении папок кнопки обновляются, а в случае наличия кнопки без проассоциированной папки, папки сдвигаются (да, в первой версии, при изначальном выборе папок, кнопки проставлялись даже не выбранным папкам - эта программа была настолько "на коленке" сделана).  

Дальше, была одна из самых глобальных доработок. У меня уже давно была написана отдельная программа (она даже в соседнем репозитории лежит), которая позволяла искать дубликаты изображений. Для поиска, она использовала примитивный алгоритм перцептивного хеша (с моими доработками, которые, правда, заключались тупо в увеличении длинны хеша). И, так сложилось, что в начале года, я реализовал отдельную библиотеку, для вычисления перцептивного хеша, методом ДКП (Дискретного Косинусного Преобразования). Та ещё мозголомная штука, но результат сравнения результатов вроде как совпадает с эталоном, с которым я сравнивал. Собственно, эту библиотеку я делал для программы тегированного просмотра изображений (там она должна была вычислять хеши картинок, для привязки по ним тегов), но в дело я её пустил только сейчас (кстати говоря, она тоже лежит тут, в отдельном репозитории). Главными пробемами старой версии программы были: кривой интерфейс (картинки отображались а размере 250х250, растягиваясь, так что рассмотреть мелкие детали было сложно); слабый алгоритм хеширования (для чёрно-белых контурных изображений поиск нормально не работал); кривой алгоритм подбора дубликатов по хешам (некоторые дубликаты просто пропускались, что я обнаружил случайно, пересортировывая часть коллекции и находя явные дубли, которых там быть не должно). Всё вышеописанное подтолкнуло меня создать новую версию поиска дубликатов, добавленную в интерфейс этого проекта. Про работу логики рассказывать не буду, а вот в интерфейсе появилась одна фишка - да, общий список изображений отображается небольшими миниатюрами, но теперь можно нажать на любую из них, и в правой части окна отобразится это изображение, на весь доступный размер. 

Ну и последняя (на данный момент) доработка. Я переделал функционал раскладки изображений, добавив возможность раскладки папок с изображениями. Суть в чём. При вбивании путей, теперь можно галочкой указать, что сканировать нужно именно папки. А, в интерфейсе раскладки изображений, в этом случае, отображается первая картинка из папки, и стралками вверх/вниз можно пролистывать картинки из этой папки. Соответственно, при нажатии на кнопку, которая проассоциирована с папкой, выбранная папка с картинками будет перемещена в целевую папку. Звучит вроде просто, но на эту доработку я убил просто тонну времени. Ну и в плюс ко всему этому я постоянно правил старый код и структуру проекта.  

#### Будущие доработки
Ну, тут всё довольно просто. Я уже начал передеклывать систему работы с горячими клавишами - с ней были некоторые технические проблемы, плюс, крайне не все кнопки были назначены как "горячие". Ещё, нужно добавить в приложение локализацию и в целом привести его интерфейс к хоть сколько-то адекватному виду. Ну и ещё несколько мелких доработок.