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

# table = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\table.txt'
# def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\def_getted.txt'
# rd_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\rd_getted.txt'
rd_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\rd.txt'
def_getted = r'C:\Users\Sofya\multidimExtrap\source\test_data\11.SquaresProducts\rd.txt'
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

with open(def_getted, 'r') as defF:
    for line in defF:
        val = line.split(';')
        # xdata_def.append(float(val[0]))
        # ydata_def.append(float(val[1]))
        # ydata1_def.append(float(val[2]))
        # ydata2_def.append(float(val[3]))

        xdata.append(float(val[0]))
        ydata.append(float(val[1]))
        zdata.append(float(val[2]))


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
# a = plt.plot(xdata_def, ydata_def, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# b = plt.plot(xdata_def, ydata1_def, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# c = plt.plot(xdata_def, ydata2_def, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)

# a = plt.plot(xdata_rd, ydata_rd, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# b = plt.plot(xdata_rd, ydata1_rd, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)
# c = plt.plot(xdata_rd, ydata2_rd, color='green', marker='o', linestyle='dashed', linewidth=2, markersize=12)

# zdata = np.array(zdata)
  #   for point in new_points:
  #       f.write("\n")
  #           f.write(" ".join(map(str, point)))

fig = plt.figure()
ax = plt.axes(projection='3d')
# # ax.contour3D(X, Y, Z, 50, cmap='binary')
ax.set_xlabel('x')
ax.set_ylabel('y')
ax.set_zlabel('z');


# ax.set_title('wireframe');

# zdata = 15 * np.random.random(100)
# # xdata = np.sin(zdata) + 0.1 * np.random.randn(100)
# # ydata = np.cos(zdata) + 0.1 * np.random.randn(100)
ax.scatter3D(xdata, ydata, zdata, c=zdata, cmap='gray');
# 
# zline = np.linspace(0, 15, 1000)
# xline = np.sin(zline)
# yline = np.cos(zline)
# ax.plot3D(xdata, ydata, zdata, 'gray')

plt.show(ax)
# plt.show(a)
# plt.show(b)
# plt.show(c)