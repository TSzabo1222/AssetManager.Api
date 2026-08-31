import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InventoryItem } from './asset.model';

const API_URL = 'https://localhost:65503/api/Inventory';

@Injectable({ providedIn: 'root' })
export class InventoryService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(API_URL);
  }

  create(item: Partial<InventoryItem>): Observable<InventoryItem> {
    return this.http.post<InventoryItem>(API_URL, item);
  }
}