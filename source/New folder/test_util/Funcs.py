import shutil


SUM = 'SUM'


def calc_new_points(point_file, output_file, next_input_file, config, func_type):
    points = []
    with open(output_file) as f:
        for line in f:
            points.append([float(i) for i in line.split(" ")])
    
    new_points = []
    if func_type == SUM:
        new_points = calc_sum(points)

    shutil.copy2(point_file, next_input_file)

    with open(next_input_file, 'a') as f:
        for point in new_points:
            f.write("\n")
            f.write(" ".join(map(str, point)))

    return points, new_points


def calc_sum(points):
    new_points = []
    for point in points:
        res = 0
        for x in point[0:-1]:
            res += x
        new_point = point.copy()
        new_point[-1] = res
        new_points.append(new_point)
    return new_points
