import { Injectable, Service, signal } from '@angular/core';

//@Service()
@Injectable({
    providedIn: 'root'
})
export class BusyService {
    loading = signal(false);
    busyRequestCount = 0;

    busy(){
        this.busyRequestCount++;
        this.loading.set(true);
    }
    idle(){
        console.log(this.loading);
        this.busyRequestCount--;
        console.log(this.busyRequestCount);
        if(this.busyRequestCount <= 0){
            this.busyRequestCount = 0;
            this.loading.set(false);
            console.log(this.loading);
        }
    }
}
