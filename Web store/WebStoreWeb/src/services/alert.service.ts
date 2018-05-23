import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar, MatSnackBarConfig } from '@angular/material';

@Injectable()
export class AlertService {

    private defaultSnackDuration: number = 1500;

    private snackBarRef: MatSnackBarRef<SimpleSnackBar>;
    private defaultConfig: MatSnackBarConfig;

    constructor(
        private snackBar: MatSnackBar
    ) {
        this.defaultConfig = {
            duration: this.defaultSnackDuration
        };
    }

    info(message: string) {
        this.snackBarRef = this.snackBar.open(message, null, this.defaultConfig);
    }
}