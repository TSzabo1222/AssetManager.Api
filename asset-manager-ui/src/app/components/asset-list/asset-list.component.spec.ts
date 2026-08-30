import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AssetService } from '../../services/asset.service';
import { Asset, AssetStatus } from '../../services/asset.model';

@Component({
  selector: 'app-asset-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './asset-list.component.html',
  styleUrl: './asset-list.component.scss'
})
export class AssetListComponent implements OnInit {
  assets: Asset[] = [];
  displayedColumns: string[] = ['name', 'type', 'serialNumber', 'status', 'assignedTo', 'actions'];

  private statusLabels: Record<AssetStatus, string> = {
    [AssetStatus.InStock]: 'Raktáron',
    [AssetStatus.Assigned]: 'Kiadva',
    [AssetStatus.Retired]: 'Selejtezve',
    [AssetStatus.Repair]: 'Javítás alatt'
  };

  private statusClasses: Record<AssetStatus, string> = {
    [AssetStatus.InStock]: 'status-instock',
    [AssetStatus.Assigned]: 'status-assigned',
    [AssetStatus.Retired]: 'status-retired',
    [AssetStatus.Repair]: 'status-repair'
  };

  constructor(private assetService: AssetService) {}

  ngOnInit(): void {
    this.loadAssets();
  }

  loadAssets(): void {
    this.assetService.getAll().subscribe({
      next: (data) => (this.assets = data),
      error: (err) => console.error('Hiba az eszközök betöltésekor:', err)
    });
  }

  statusLabel(status: AssetStatus): string {
    return this.statusLabels[status] ?? 'Ismeretlen';
  }

  statusClass(status: AssetStatus): string {
    return this.statusClasses[status] ?? '';
  }
}