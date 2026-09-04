import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ActivityService } from '../../services/activity.service';
import { ActivityLog } from '../../services/asset.model';

@Component({
  selector: 'app-activity-panel',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './activity-panel.component.html',
  styleUrl: './activity-panel.component.scss'
})
export class ActivityPanelComponent implements OnInit {
  isOpen = false;
  logs: ActivityLog[] = [];
  isLoading = false;

  constructor(private activityService: ActivityService) {}

  ngOnInit(): void {
    this.load();
  }

  toggle(): void {
    this.isOpen = !this.isOpen;
    if (this.isOpen) {
      this.load();
    }
  }

  load(): void {
    this.isLoading = true;
    this.activityService.getRecent(30).subscribe({
      next: (data) => {
        this.logs = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading activity log:', err);
        this.isLoading = false;
      }
    });
  }

  iconFor(action: string): string {
    switch (action) {
      case 'created': return 'add_circle';
      case 'deleted': return 'delete';
      case 'updated': return 'edit';
      case 'assigned': return 'person_add';
      case 'returned': return 'assignment_return';
      default: return 'info';
    }
  }
}