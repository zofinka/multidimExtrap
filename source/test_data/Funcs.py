import shutil


SUM = 'SUM'


def calc_new_points(point_file, output_file, next_input_file, config, func_type):
    points = []
    with open(output_file) as f:
        for line in f:
            points.append([float(i) for i in line.split(" ")])

    if func_type == SUM:
        calc_sum(points)

    shutil.copy2(point_file, next_input_file)

    with open(next_input_file, 'a') as f:
        for point in points:
            f.write("\n")
            f.write(" ".join(map(str, point)))


def calc_sum(points):
    for point in points:
        res = 0
        for x in point:
            res += x
        point[-1] = res