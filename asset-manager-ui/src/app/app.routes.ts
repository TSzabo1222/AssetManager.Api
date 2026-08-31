import { Routes } from '@angular/router';
import { AssetListComponent } from './components/asset-list/asset-list.component';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { InventoryListComponent } from './components/inventory-list/inventory-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'assets', pathMatch: 'full' },
  { path: 'assets', component: AssetListComponent },
  { path: 'employees', component: EmployeeListComponent },
  { path: 'inventory', component: InventoryListComponent }
];