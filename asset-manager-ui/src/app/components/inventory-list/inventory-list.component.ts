import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { InventoryService } from '../../services/inventory.service';
import { InventoryItem } from '../../services/asset.model';
import { AddInventoryDialogComponent } from '../add-inventory-dialog/add-inventory-dialog.component';

@Component({
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule],
  templateUrl: './inventory-list.component.html',
  styleUrl: './inventory-list.component.scss'
})
export class InventoryListComponent implements OnInit {
  items: InventoryItem[] = [];
  displayedColumns: string[] = ['name', 'sku', 'quantity', 'category', 'location', 'actions'];

  constructor(private inventoryService: InventoryService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.inventoryService.getAll().subscribe({
      next: (data) => (this.items = data),
      error: (err) => console.error('Error loading inventory:', err)
    });
  }

  openAddDialog(): void {
    const ref = this.dialog.open(AddInventoryDialogComponent);
    ref.afterClosed().subscribe((newItem) => {
      if (!newItem) return;
      this.inventoryService.create(newItem).subscribe({
        next: () => this.load(),
        error: (err) => console.error('Error creating inventory item:', err)
      });
    });
  }

  openEditDialog(item: InventoryItem): void {
    const ref = this.dialog.open(AddInventoryDialogComponent, { data: item });
    ref.afterClosed().subscribe((updated) => {
      if (!updated) return;
      this.inventoryService.update(item.id, { ...updated, id: item.id }).subscribe({
        next: () => this.load(),
        error: (err) => console.error('Error updating inventory item:', err)
      });
    });
  }

  deleteItem(item: InventoryItem): void {
    if (!confirm(`Delete "${item.name}"? This cannot be undone.`)) return;
    this.inventoryService.delete(item.id).subscribe({
      next: () => this.load(),
      error: (err) => console.error('Error deleting inventory item:', err)
    });
  }
}