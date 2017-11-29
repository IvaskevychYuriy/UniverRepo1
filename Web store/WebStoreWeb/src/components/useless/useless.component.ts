import { Component, OnInit, OnDestroy } from '@angular/core';

import { UselessTask } from '../../models/useless-task';
import { UselessTasksService } from '../../services/useless-tasks.service';

@Component({
    moduleId: module.id.toString(),
    selector: 'useless',
    templateUrl: 'useless.component.html',
    styleUrls: ['useless.component.css']
})
export class UselessComponent implements OnInit, OnDestroy{
    private timeoutHandlerId: number;
    private workAmount: number;

    public tasks: UselessTask[] = [];

    constructor(
        private uselessTasksService: UselessTasksService
    ) { }
    
    ngOnInit(): void {
        this.tasks = [];
        const that = this;
        this.timeoutHandlerId = setTimeout(() => that.updateTasks(), 1000);
    }

    private updateTasks() {
        if (this.tasks) {
            for (let i = 0; i < this.tasks.length; ++i) {
                const task = this.tasks[i];
                if (task.status === 0 || task.status === 1) {
                    this.uselessTasksService.get(task.id, task.serverIdentifier)
                        .then(updatedTask => {
                            this.tasks[i].progress = updatedTask.progress;
                            this.tasks[i].status = updatedTask.status;
                        });
                }
            }
        }
        
        const that = this;
        this.timeoutHandlerId = setTimeout(() => that.updateTasks(), 1000);
    }

    ngOnDestroy(): void {
        clearTimeout(this.timeoutHandlerId);
    }
    
    public addNew() {
        if (!this.workAmount) {
            return;
        }
        
        this.uselessTasksService.add(this.workAmount)
            .then(task => this.tasks.push(task));
    }

    public cancel(id: number) {
        const index = this.tasks.findIndex(task => task.id === id);
        if (index === -1) {
            return;
        }
        
        this.uselessTasksService.cancel(this.tasks[index].id, this.tasks[index].serverIdentifier)
            .then(task => this.tasks.splice(index, 1))
            .catch(task => this.tasks.splice(index, 1));
    }
}