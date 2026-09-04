import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ActivityLog } from './asset.model';

const API_URL = 'https://localhost:65503/api/Activity';

@Injectable({ providedIn: 'root' })
export class ActivityService {
  constructor(private http: HttpClient) {}

  getRecent(take: number = 30): Observable<ActivityLog[]> {
    return this.http.get<ActivityLog[]>(`${API_URL}?take=${take}`);
  }
}