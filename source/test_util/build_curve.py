from mpl_toolkits import mplot3d

import matplotlib.pyplot as plt
import numpy as np

import math


# zdata = []
xdata = []
ydata = []

table = r'C:\Users\Sofya\multidimExtrap\source\test_data\20.LGFunc\table.txt'
with open(table, 'r') as tFile:
    for line in tFile:
        val = line.split(';')
        xdata.append(float(val[0]))
        ydata.append(math.sqrt(float(val[1])^2 + float(val[2])^2 + float(val[3])^2))

        # xdata.append(float(val[1]))
        # ydata.append(float(val[2]))
        # zdata.append(float(val[3]))

xdata = np.array(xdata)
ydata = np.array(ydata)
# zdata = np.array(zdata)
  #   for point in new_points:
  #       f.write("\n")
  #           f.write(" ".join(map(str, point)))

# x = np.linspace(-6, 6, 30)
# y = np.linspace(-6, 6, 30)

# X, Y = np.meshgrid(x, y)
# Z = f(X, Y)

fig = plt.figure()
# ax = plt.axes(projection='3d')
ax = plt.axes(projection='2d')
# ax.contour3D(X, Y, Z, 50, cmap='binary')
# ax.set_xlabel('x')
# ax.set_ylabel('y')
# ax.set_zlabel('z');

# ax.plot_wireframe(X, Y, Z, color='black')
# ax.set_title('wireframe');

# zdata = 15 * np.random.random(100)
# # xdata = np.sin(zdata) + 0.1 * np.random.randn(100)
# # ydata = np.cos(zdata) + 0.1 * np.random.randn(100)
ax.scatter3D(xdata, ydata, c=zdata, cmap='Greens');

# zline = np.linspace(0, 15, 1000)
# xline = np.sin(zline)
# yline = np.cos(zline)
# ax.plot3D(xdata, ydata, zdata, 'gray')

plt.show(ax)