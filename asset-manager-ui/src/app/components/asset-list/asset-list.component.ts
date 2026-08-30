import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AssetService } from '../../services/asset.service';
import { Asset, AssetStatus } from '../../services/asset.model';
import { AssignDialogComponent } from '../assign-dialog/assign-dialog.component';
import { AddAssetDialogComponent } from '../add-asset-dialog/add-asset-dialog.component';

@Component({
  selector: 'app-asset-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule],
  templateUrl: './asset-list.component.html',
  styleUrl: './asset-list.component.scss'
})
export class AssetListComponent implements OnInit {
  assets: Asset[] = [];
  displayedColumns: string[] = ['name', 'type', 'serialNumber', 'status', 'assignedTo', 'actions'];

  private statusLabels: Record<AssetStatus, string> = {
    [AssetStatus.InStock]: 'In stock',
    [AssetStatus.Assigned]: 'Assigned',
    [AssetStatus.Retired]: 'Retired',
    [AssetStatus.Repair]: 'In repair'
  };

  private statusClasses: Record<AssetStatus, string> = {
    [AssetStatus.InStock]: 'status-instock',
    [AssetStatus.Assigned]: 'status-assigned',
    [AssetStatus.Retired]: 'status-retired',
    [AssetStatus.Repair]: 'status-repair'
  };

  constructor(private assetService: AssetService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadAssets();
  }

  loadAssets(): void {
    this.assetService.getAll().subscribe({
      next: (data) => (this.assets = data),
      error: (err) => console.error('Error loading assets:', err)
    });
  }

  statusLabel(status: AssetStatus): string {
    return this.statusLabels[status] ?? 'Unknown';
  }

  statusClass(status: AssetStatus): string {
    return this.statusClasses[status] ?? '';
  }

  openAddAssetDialog(): void {
    const ref = this.dialog.open(AddAssetDialogComponent);

    ref.afterClosed().subscribe((newAsset) => {
      if (!newAsset) return;
      this.assetService.create(newAsset).subscribe({
        next: () => this.loadAssets(),
        error: (err) => console.error('Error creating asset:', err)
      });
    });
  }

  openManageDialog(asset: Asset): void {
    const ref = this.dialog.open(AssignDialogComponent, { data: { asset } });

    ref.afterClosed().subscribe((result) => {
      if (!result) return;

      if (result.action === 'assign') {
        this.assetService.assign(asset.id, result.employeeId).subscribe({
          next: () => this.loadAssets(),
          error: (err) => console.error('Error assigning asset:', err)
        });
      } else if (result.action === 'return') {
        this.assetService.returnAsset(asset.id).subscribe({
          next: () => this.loadAssets(),
          error: (err) => console.error('Error returning asset:', err)
        });
      }
    });
  }
}