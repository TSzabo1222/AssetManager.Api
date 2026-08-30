import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AssetListComponent } from './components/asset-list/asset-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AssetListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'asset-manager-ui';
}
