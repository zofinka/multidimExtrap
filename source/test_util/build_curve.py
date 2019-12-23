from mpl_toolkits import mplot3d

import matplotlib.pyplot as plt
import numpy as np

import math


# zdata = []
xdata_def = []
ydata_def = []
ydata1_def = []
ydata2_def = []

xdata = []
ydata = []
zdata = []


xdata_rd = []
ydata_rd = []
ydata1_rd = []
ydata2_rd = []

# def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\table.txt'
# def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\quicker_best_ever_0.009.txt'
# rd_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\rd_getted.txt'
# def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\rf_0.09.txt'
# def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\11.SquaresProducts\rf_0.9.txt'
# def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\12.sincos\def.txt'

# with open(table, 'r') as tFile:
#     for line in tFile:
#         val = line.split(';')
#         xdata.append(float(val[0]))
        # ydata.append(math.sqrt(float(val[1])**2 + float(val[2])**2 + float(val[3])**2))
        # ydata.append(float(val[1]))
        # ydata1.append(float(val[2]))
        # ydata2.append(float(val[3]))

        # xdata.append(float(val[1]))
        # ydata.append(float(val[2]))
        # zdata.append(float(val[3]))


# with open(rd_getted, 'r') as rdF:
#     for line in rdF:
        # val = line.split(';')
        # xdata_rd.append(float(val[0]))
        # ydata_rd.append(float(val[1]))
        # ydata1_rd.append(float(val[2]))
        # ydata2_rd.append(float(val[3]))

        # xdata.append(float(val[0]))
        # ydata.append(float(val[1]))
        # zdata.append(float(val[2]))

# with open(def_getted, 'r') as defF:
#     i = 1
#     for line in defF:
#         val = line.split(';')
#         # if i in [186, 187, 205, 207, 135]:
#         # if i in []:
#         #     xdata_rd.append(float(val[0]))
#         #     ydata_rd.append(float(val[1]))
#         #     ydata1_rd.append(float(val[2]))
#         #     ydata2_rd.append(float(val[3]))

#         # else:
#         #     xdata_def.append(float(val[0]))
#         #     ydata_def.append(float(val[1]))
#         #     ydata1_def.append(float(val[2]))
#         #     ydata2_def.append(float(val[3]))

#         # xdata_def.append(float(val[0]))
#         # ydata_def.append(float(val[1]))
#         # ydata1_def.append(float(val[2]))
#         # ydata2_def.append(float(val[3]))

#         # if i in range(1566, 1574):
#         xdata.append(float(val[0]))
#         ydata.append(float(val[1]))
#         zdata.append(float(val[2]))
#         i += 1
#         # if i > 19:
#             # break

# for i in range(len(xdata_def) - 1):
#     for j in range(i+1, len(xdata_def)):
#         # print(i);
#         # if (xdata_def[i] == xdata_def[j] and ydata_def[i] == ydata_def[j] and ydata1_def[i] == ydata1_def[j] and ydata2_def[j] == ydata2_def[i]):
#         if (xdata[i] == xdata[j] and ydata[i] == ydata[j] and zdata[i] == zdata[j]):
#             print(i, j);


# xdata_def = np.array(xdata_def)
# ydata_def = np.array(ydata_def)
# ydata1_def = np.array(ydata1_def)
# ydata2_def = np.array(ydata2_def)

# xdata_rd = np.array(xdata_rd)
# ydata_rd = np.array(ydata_rd)
# ydata1_rd = np.array(ydata1_rd)
# ydata2_rd = np.array(ydata2_rd)
# ydata2 = np.array(ydata2)
# a = plt.plot(xdata, ydata, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# b = plt.plot(xdata, ydata1, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# c = plt.plot(xdata, ydata2, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# a = plt.plot(xdata_def, ydata_def, color='green', marker='o', linestyle='dashed', markersize=3)
# a = plt.scatter(xdata_def, ydata_def, s=10)
# b = plt.scatter(xdata_def, ydata1_def, s=10)
# c = plt.scatter(xdata_def, ydata2_def, s=10)

# a1 = plt.scatter(xdata_rd, ydata_rd, s=10)
# b1 = plt.scatter(xdata_rd, ydata1_rd, s=10)
# c1 = plt.scatter(xdata_rd, ydata2_rd, s=10)

# a = plt.plot(xdata_rd, ydata_rd, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# b = plt.plot(xdata_rd, ydata1_rd, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# c = plt.plot(xdata_rd, ydata2_rd, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)

# zdata = np.array(zdata)
  #   for point in new_points:
  #       f.write("\n")
  #           f.write(" ".join(map(str, point)))

# fig = plt.figure()
# ax = plt.axes(projection='3d')
# # # # ax.contour3D(X, Y, Z, 50, cmap='binary')
# ax.set_xlabel('x')
# ax.set_ylabel('y')
# ax.set_zlabel('z');


# ax.set_title('wireframe');

# zdata = 15 * np.random.random(100)
# # xdata = np.sin(zdata) + 0.1 * np.random.randn(100)
# # ydata = np.cos(zdata) + 0.1 * np.random.randn(100)
# ax.scatter3D(xdata, ydata, zdata, c=zdata, cmap='Dark2');
# 
# zline = np.linspace(0, 15, 1000)
# xline = np.sin(zline)
# yline = np.cos(zline)
# ax.plot3D(xdata, ydata, zdata, 'gray')

# plt.show(ax)
# plt.show(a)
# plt.show(b)
# plt.show(c)

# plt.show(a1)
# plt.show(b1)
# plt.show(c1)

x = [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200]
# x = [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, .., 1570]
x = [i for i in range(50, 301, 10)]
x = [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 240, 270, 300]

# grid
# y = [
#  43.6828021553744,
#  15.3714601867758,
#  7.88336733014003,
#  5.20459074076361,
#  3.90166432340011,
#  3.09514798410188,
#  2.54032859694253,
#  2.22390615187869,
#  1.99365310821493,
#  1.77487303332627,
#  1.63800078230471,
#  1.56019840201392,
#  1.53018298719081,
#  1.49652421755528,
#  1.48412143617082,
#  1.47292227779929]

# grid 300
# y = [
# 43.6828021553744,
# 15.3714601867758,
# 7.88336733014003,
# 5.20459074076361,
# 3.90166432340011,
# 3.09514798410188,
# 2.54032859694253,
# 2.22390615187869,
# 1.99365310821493,
# 1.77487303332627,
# 1.63800078230471,
# 1.56019840201392,
# 1.53018298719081,
# 1.49652421755528,
# 1.48412143617082,
# 1.47292227779929,    
# 0.829178947042786,   210
# 8.6108047788384,    220
# 8.90106507356746,   230
# 0.613448177974714,   240 g
# 9.43928036409427,   250
# 12.7368135717228,   260
# 0.500970321900731,  270  g
# 13.0444464901396, 280
# 13.1903157280584, 290
# 0.429300215358677] g

y = [
43.6828021553744,
15.3714601867758,
7.88336733014003,
5.20459074076361,
3.90166432340011,
3.09514798410188,
2.54032859694253,
2.22390615187869,
1.99365310821493,
1.77487303332627,
1.63800078230471,
1.56019840201392,
1.53018298719081,
1.49652421755528,
1.48412143617082,
1.47292227779929,    
0.829178947042786,  
0.613448177974714, 
0.500970321900731,
0.429300215358677]

# det bad points
y1 = [
54.9074744497854,
39.1965113947326,
20.7384601643277,
16.6256723830459,
12.6714376169997,
12.1876596688948,
9.26321689878929,
8.09814918644841,
7.21038495446966,
5.96938752753181,
5.47731505130816,
4.84330236553283,
4.16175120680475,
3.58311841724456,
3.21487624993761,
2.805273964174,
2.54443324520506,
# 2.42445342325213,
# 2.25991261634969,
2.08360032741432,
# 1.97589258158506,
# 1.7773078978005,
1.61961512976722,
# 1.50453256848294,
# 1.40725461331366,
1.29749791510929,
]

# det alg
# y1 = [
# 41.2420162147231,
# 32.4595164273526,
# 30.3847140309057,
# 21.0862069987996,
# 17.9561805431752,
# 12.8883247537433,
# 8.63005286842741,
# 7.25278614699825,
# 8.26432691347248,
# 7.60454866574947,
# 5.09865585103635,
# 4.97653081732062,
# 4.68757737118159,
# 3.94584366318679,
# 3.23204232045822,
# 2.88592029647294]


# rf alg
y2 = [
51.6180992766958,
42.5662281175501,
38.9260356608254,
35.0731509405805,
19.3523894467473,
15.2115520860846,
15.8657024002856,
14.0792141989755,
14.7859481753171,
13.2209091718467,
13.0006435452094,
11.1122082400973,
10.9894104658851,
10.7665986876792,
10.6277828675183,
9.40307106573177,
6.09625101008944,
# 5.32622492034817,
# 5.09955120626776,
4.61687435979194,
# 3.94378063458568,
# 3.56237133336386,
3.2203886878541,
# 2.73127220473384,
# 2.55197872612131,
2.37003632996878]

# y = [0.0156925902777177, 0.0125642945040645, 0.0111129391871742, 0.0075907637090179, 
#      0.00946467508885396, 4.05087788527739E-16, 0.00735729835901483, 0.0055514069556036, 
#      0.00614896797771756, 0.00305606570720735, 0.00595248833401095, 0.00477099756137324, 
#      0.00530892474245516, 0.00303304186592916, 0.00493686786599091, 0.0077562951802588]
# y1 = [0.0216791024563991, 0.0147964208801749, 0.012232490332866, 0.0113819845604111, 
#       0.0110465325448213, 0.0104351133960394, 0.00971025911748898, 0.00666984800859241,
#       0.00593408996914535, 0.00589942116577096, 0.00565893612797305, 0.0051869457241948,
#       0.0049492104170317, 0.00483759970299873, 0.0048261488977986, 0.00557216461357879]
# y2 = [0.0165869938344834,
#  0.0280590438299869,
#  0.014395732220811,
#  0.0154665870237087,
#  0.0575213486734638,
#  0.0155985989541642,
#  0.0162785112698954,
#  0.0156448079459047,
#  0.0244942697208247,
#  0.0164658456205658,
#  0.0166719733145704,
#  0.0324153711176724,
#  0.0168399318230271,
#  0.00959169711464969,
#  0.0118012948392963,
#  0.0161721058987652]


# on grid
# y = [0.0148293938773319,
# 0.0123949400156025,
# 0.0111077145082084,
# 0.00912455283154883,
# 0.00953224004671374,
# 0.00754745853638844,
# 0.00804146852176463,
# 0.00613516554478711,
# 0.00619562551116879,
# 0.00531623830859407,
# 0.00512256123325119,
# 0.00495753206609612,
# 0.00451056048939465,
# 0.00495623191276511,
# 0.00413879262037173,
# 0.00457279569866243]


# LG
# calc on grid 
# y = [
# 0.0135343853739278,
# 0.00881717924652286,
# 0.00773090994928512,
# 0.00818061153833739,
# 0.00503310283754104,
# 0.00537994375549748,
# 0.00507780944420792,
# 0.00356727893879998,
# 0.00451474191406934,
# 0.00368383411748252,
# 0.00321779565009742,
# 0.00440197916623724,
# 0.00317120132494727,
# 0.00323050921110061,
# 0.00348275366235547,
# 0.00279893426240151,
# ]

# # rf alg 
# y3 = [
# 0.104380427080039,
# 0.0876480877677509,
# 0.0804830257327009,
# 0.0919714103953595,
# 0.0976218129748583,
# 0.0810022070293435,
# 0.0768844525386724,
# 0.0720986339598671,
# 0.0610454080949617,
# 0.0513263593079124,
# 0.0526889999770739,
# 0.0430058373318683,
# 0.04193284461842,
# 0.039888157272914,
# 0.0392025818492667,
# 0.0397858899210044,
# ]

# # det
# y4 =  [
# 0.0749735153575725,
# 0.0587634596472714,
# 0.0504872073189415,
# 0.0510989780409803,
# 0.0414167345512711,
# 0.0296097867857443,
# 0.0290046630210783,
# 0.0232560823043265,
# 0.0196814595530825,
# 0.0195351878989268,
# 0.0194618571005184,
# 0.0168521414337327,
# 0.0152739967370207,
# 0.0149684307430457,
# 0.0156523187132724,
# 0.0129788919399587,
# ]


# sincos
# grid
# y = [
# 0.447011308577491,
# 0.453359013387118,
# 0.467136347307877,
# 0.475899488464691,
# 0.459633042540261,
# 0.4375257951523,
# 0.455498761995633,
# 0.483308605619756,
# 0.485614860051736,
# 0.479456410986238,
# 0.484194481710859,
# 0.490417856924981,
# 0.499361840694611,
# 0.487776053898211,
# 0.46610827284222,
# 0.446597531226828,
# 0.43427180937759,
# 0.425485441192678,
# 0.41989505745052,
# 0.415760048387006,
# 0.413611938730089,
# 0.411393889032687,
# 0.409877816937165,
# 0.411303324033443,
# 0.408748027827664,
# 0.407370823479653,
# ]

# det 
# y1 =[
# 2.69564777895289,
# 2.39786692883272,
# 2.79236541533656,
# 2.04494419235699,
# 1.82666615813791,
# 1.47469857902024,
# 1.58165013218683,
# 1.43199377890258,
# 1.69200638606225,
# 1.50829956601119,
# 1.4383441456729,
# 1.44958452717997,
# 1.40085686737783,
# 1.30396835915871,
# 1.23488922204536,
# 1.17140192361238,
# 1.13245946140444,
# 1.07443598136512,
# 1.02611652068871,
# 0.975355247273073,
# 0.938598500546521,
# 0.94508551502124,
# 0.843717906529986,
# 0.815316245853266,
# 0.79313106386066,
# 0.786821855749665,
# ]

# rf
# y2 = [
# 0.882147374570425,
# 0.589031705446708,
# 0.618020530018165,
# 0.671463204612057,
# 0.615798030749472,
# 0.635742355634504,
# 0.611446333229298,
# 0.654971742210782,
# 0.649461262300503,
# 0.61919294960441,
# 0.622705616501679,
# 0.611908449129776,
# 0.661656242750644,
# 0.646716517976391,
# 0.683470268901719,
# 0.696361090287254,
# 0.678138778193523,
# 0.681812039885624,
# 0.686796043684633,
# 0.669208476204385,
# 0.66176619293304,
# 0.680467396172817,
# 0.701180081541113,
# 0.737143032182795,
# 0.750746068398313,
# 0.756889404983855,
# ]

# a = plt.plot(x[0::3], y[0::3], color='green', marker='o', linestyle='dashed', linewidth=1, markersize=1)
# b = plt.plot(x[0::3], y1[0::3], color='blue', marker='o', linestyle='dashed', linewidth=1, markersize=1)
# c = plt.plot(x[0:len(y2)], y2, color='red', marker='o', linestyle='dashed', linewidth=2, markersize=4)

# a = plt.plot(x[0:len(y2)], y[0:len(y2)], color='green', marker='o', linestyle='dashed', linewidth=1, markersize=2)
# b = plt.plot(x[0:len(y2)], y1[0:len(y2)], color='blue', marker='o', linestyle='dashed', linewidth=1, markersize=2)
# c = plt.plot(x[0:len(y2)], y2, color='red', marker='o', linestyle='dashed', linewidth=1, markersize=2)

# a = plt.plot(x[:16], y[:16], color='green', marker='o', linestyle='dashed', linewidth=1, markersize=2)
# b = plt.plot(x[:16], y1[:16], color='blue', marker='o', linestyle='dashed', linewidth=1, markersize=2)
# c = plt.plot(x[:16], y2[:16], color='red', marker='o', linestyle='dashed', linewidth=1, markersize=2)


a = plt.plot(x, y, color='green', marker='o', linestyle='dashed', linewidth=1, markersize=2)
b = plt.plot(x, y1, color='blue', marker='o', linestyle='dashed', linewidth=1, markersize=2)
c = plt.plot(x, y2, color='red', marker='o', linestyle='dashed', linewidth=1, markersize=2)

# b = plt.plot(xdata, ydata1, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# c = plt.plot(xdata, ydata2, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
plt.show(a)
plt.show(b)
plt.show(c)




# пз1:
# метод наименьших квадратов
# опорные точки - шепард 
# персептрон на градиентном спуске
# случайный лес 


# пз2:
# параметры, данные, пример данных 

# пз3:
# добавить HLD 

# сделать: 
# руководство системного 

