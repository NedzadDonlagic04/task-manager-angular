import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import TaskReadDTO from '../../dtos/task-read.dto';
import { TaskService } from '../../services/task.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-view-tasks-page',
  imports: [DatePipe],
  templateUrl: './view-tasks-page.html',
  styleUrl: './view-tasks-page.css',
})

export class ViewTasksPage implements OnInit
{
 
  tasks: TaskReadDTO[]=[];

  constructor(private TaskService: TaskService) {}
  
  ngOnInit(): void 
  {
    this.TaskService.getTasks().subscribe({
      next: (tasks: TaskReadDTO[]) => 
      {
        console.log(tasks);
        this.tasks = tasks;
      },
      error: (error) => 
      {
        console.error('Error fetching tasks:', error);
      }
    });
  }
}
