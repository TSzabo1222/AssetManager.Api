import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Asset } from './asset.model';

const API_URL = 'https://localhost:65503/api/Assets';

@Injectable({ providedIn: 'root' })
export class AssetService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Asset[]> {
    return this.http.get<Asset[]>(API_URL);
  }

  create(asset: Partial<Asset>): Observable<Asset> {
    return this.http.post<Asset>(API_URL, asset);
  }

  assign(assetId: number, employeeId: number): Observable<Asset> {
    return this.http.post<Asset>(`${API_URL}/${assetId}/assign`, { employeeId });
  }

  returnAsset(assetId: number): Observable<Asset> {
    return this.http.post<Asset>(`${API_URL}/${assetId}/return`, {});
  }
}