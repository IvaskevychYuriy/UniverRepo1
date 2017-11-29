using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Api.DataTransferObjects;

namespace WebStore.Api
{
    public static class UselessTaskManager
    {
        private static List<UselessTaskDTO> _uselessTasks;
        private static List<KeyValuePair<int, Task>> _uselessTasksHandlers;
        private static int _taskId;
        private const int _endProgress = 100;
        private const int _checkDelay = 100;
        private static Object _lock;

        public static string ServerIdentifier { get; set; }

        static UselessTaskManager()
        {
            _uselessTasks = new List<UselessTaskDTO>();
            _uselessTasksHandlers = new List<KeyValuePair<int, Task>>();
            _taskId = 0;
            _lock = new Object();
        }

        public static UselessTaskDTO Get(int id)
        {
            UselessTaskDTO task = null;

            lock (_lock)
            {
                int index = _uselessTasks.FindIndex(t => t.Id == id);
                if (index != -1)
                {
                    task = _uselessTasks[index].Clone() as UselessTaskDTO;
                }
            }

            return task;
        }

        public static UselessTaskDTO StartNew(long workAmount)
        {
            var task = new UselessTaskDTO()
            {
                Status = UselessTaskStates.New,
                Progress = 0,
                TotalWorkAmount = workAmount,
                ServerIdentifier = ServerIdentifier
            };

            lock (_lock)
            {
                int id = _taskId++;
                task.Id = id;
                _uselessTasks.Add(task);
            }

            int tid = task.Id;
            var worker = Task.Run(async () =>
            {
                InitWorkerVariables(tid, out int i, out UselessTaskStates state, out long progress, out long work);
                while (progress < _endProgress && state == UselessTaskStates.Running)
                {
                    await Task.Delay(_checkDelay);
                    UpdateProgress(tid, ref i, ref work, ref progress);
                }
                await Stop(tid);
            });

            lock (_lock)
            {
                _uselessTasksHandlers.Add(new KeyValuePair<int, Task>(tid, worker));
            }

            return task;
        }

        public static long ReportProgress(int id)
        {
            lock (_lock)
            {
                int index = _uselessTasks.FindIndex(t => t.Id == id);
                if (index != -1)
                {
                   return  _uselessTasks[index].Progress;
                }
            }

            return 0;
        }

        public static async Task Stop(int id)
        {
            lock (_lock)
            {
                int index = _uselessTasks.FindIndex(t => t.Id == id);
                if (index != -1)
                {
                    _uselessTasks[index].Status = UselessTaskStates.Canceled;
                }
            }

            await Task.Delay(_checkDelay * 2);

            lock(_lock)
            {
                int index = _uselessTasksHandlers.FindIndex(kvp => kvp.Key == id);
                if (index != -1)
                {
                    _uselessTasksHandlers.RemoveAt(index);
                }
            }
        }

        private static void InitWorkerVariables(long tid, out int i, out UselessTaskStates state, out long progress, out long work)
        {
            i = -1;
            progress = _endProgress;
            work = 0;
            state = UselessTaskStates.Running;

            lock (_lock)
            {
                i = _uselessTasks.FindIndex(t => t.Id == tid);
                if (i == -1 || _uselessTasks[i] == null || _uselessTasks[i].Progress >= _endProgress)
                {
                    return;
                }

                _uselessTasks[i].Status = state;
                progress = _uselessTasks[i].Progress;
                work = progress * _uselessTasks[i].TotalWorkAmount / _endProgress;
            }
        }

        private static void UpdateProgress(long tid, ref int i, ref long work, ref long progress)
        {
            lock (_lock)
            {
                i = _uselessTasks.FindIndex(t => t.Id == tid);
                if (i == -1 || _uselessTasks[i] == null || _uselessTasks[i].Status == UselessTaskStates.Done || _uselessTasks[i].Status == UselessTaskStates.Canceled || _uselessTasks[i].Progress >= _endProgress)
                {
                    return;
                }

                work += _checkDelay;
                _uselessTasks[i].Progress = work * _endProgress / _uselessTasks[i].TotalWorkAmount;
                progress = _uselessTasks[i].Progress;

                if (_uselessTasks[i].Progress >= _endProgress)
                {
                    _uselessTasks[i].Status = UselessTaskStates.Done;
                    return;
                }
            }
        }
    }
}
