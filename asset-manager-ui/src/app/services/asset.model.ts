export enum AssetStatus {
  InStock = 0,
  Assigned = 1,
  Retired = 2,
  Repair = 3
}

export interface Employee {
  id: number;
  fullName: string;
  email: string;
  department: string;
  position: string;
  hireDate: string;
}

export interface Asset {
  id: number;
  name: string;
  type: string;
  serialNumber: string;
  purchaseDate: string;
  status: AssetStatus;
  assignedToEmployeeId?: number | null;
  assignedToEmployee?: Employee | null;
}

export interface InventoryItem {
  id: number;
  name: string;
  sku: string;
  quantity: number;
  category: string;
  location: string;
}