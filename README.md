## Name: Elija de Hoog
## Student Number: 37121209

## Part 1:
## 1.
These timings were made with 12 threads and averaged over 5 runs
```
Array size      Time (Multi-Threaded)       Time (Single)
10^1            223                         0
10^2            222                         0
10^3            225                         0
10^4            256                         7
10^5            287                         75
10^6            473                         609
10^7            2016                        4367
```
## 2. 
Based on the number of cores on my computer (6) I expected a speed up of 6x when implementing multi-threading in the program.

## 3.
This speed up was not achieved in my programs. I beleive that the overhead time and memory required to set up the threads by kernel contribute to an overall slower execution than expected. Also there could be implementation errors in my code or inefficiencies I overlooked.

## 4.
```
Number of Threads       Sp
1                       0.903
2                       1.314
3                       1.347
4                       1.757
5                       1.626
6                       2.132
7                       1.710
8                       2.003
9                       1.656
10                      1.942
11                      1.570
12                      1.760
24                      0.970
100                     0.374
```
My laptop has 6 cores, the Sp fluctuates do to the variations in run times each run. For more accurate speed factors I would need to average more runs.

Part 2:
These timings were made with 12 threads and averaged over 5 runs
```
Array size      Time (Multi-Threaded)       Time (Single)       Sp
10^3            88                          20                  0.227
10^4            89                          22                  0.247
10^5            120                         36                  0.300
10^6            275                         145                 0.527
10^7            580                         1271                2.191
10^8            2994                        12813               3.936
```
## 1.
Splitting up work between threads is only useful for specific situations. This is depedning n the amount of elements involved in the process you want to split into threads. For a small job the increase in overhead and memory used by the kernel will result in a slower exucution time when compared with main thread operation.

## 2.
The implications that result from concurent code consist of ensureing that the job size is large enough to make it beneficial. Also you need to ensure critical sections of the code are protected when implementing concurency. If these critical sections are not protected it can lead to very large problems in the program.

## 3.
Considering that in my testing I found using a sample size of 10^8 resulted in accuracy to 3 decimal places I would guess that a sample size on the order of 10^19 would be required to estimate pi to 7 decimal places. A problem with such a large sample size is CPU processing capabilities as well as RAM capabilities on laptops and home PC's. The accuracy of the Monte Carlo Simulation in estimating pi is depedent on the sample size. The more sample points that are passed to the simulation the more accurate the estimations made by the simulation will be. Another thing to consider would be the actual randomess of the random number generator used, this could be a limitation to the simulation when considering high accuracy estimations as more accurate estimations would require completely unbiased random sample points.
