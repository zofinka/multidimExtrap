import os
import shutil
import subprocess

import argparse

from test_data.Funcs import SUM, calc_new_points


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--config', type=str, required=True)
    parser.add_argument('--points', type=str, required=True)
    parser.add_argument('--runNum', type=int, default=10)
    parser.add_argument('--funcType', choices=[SUM], default=SUM)

    args = parser.parse_args()

    if not os.path.exists(args.config):
        print(args.config + " is not valid path")
        exit()

    if not os.path.exists(args.points):
        print(args.points + " is not valid path")
        exit()

    current_dir = os.path.dirname(os.path.abspath(__file__))
    test_data_dir = os.path.join(current_dir, "test_data", "output")
    if not os.path.exists(test_data_dir):
        os.mkdir(test_data_dir)

    # copy to test_data folder to collect all files and logs there
    if args.points != os.path.join(test_data_dir, 'points_0.txt'):
        shutil.copy(args.points, os.path.join(test_data_dir, 'points_0.txt'))


    solver = os.path.join(current_dir, "Solver", "bin", "Debug", "Solver.exe")

    for i in range(args.runNum):
        input_points = os.path.join(test_data_dir, "points_" + str(i) + ".txt")
        output_points = os.path.join(test_data_dir, "output_points_" + str(i) + ".txt")
        os.system("{0} {1} {2} {3}".format(solver, args.config, input_points, output_points))
        if not os.path.exists(output_points):
            break

        next_input_points = os.path.join(test_data_dir, "points_" + str(i + 1) + ".txt")
        calc_new_points(input_points, output_points, next_input_points, args.config, args.funcType)

