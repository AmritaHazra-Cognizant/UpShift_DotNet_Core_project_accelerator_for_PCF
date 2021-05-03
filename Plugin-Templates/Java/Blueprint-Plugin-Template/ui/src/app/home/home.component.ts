import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from '../common-services/common.service';
import { StorageService } from '../common-services/storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  processInstanceId = '';
  projectName = '';
  jwt = '';
  backendUrl = '';
  
  constructor(private route: Router,
    private activatedRoute: ActivatedRoute,
    private http: HttpClient,
    private commonService: CommonService,
    private storageService: StorageService) {
      this.backendUrl = commonService.getBackendUrl();
  }

  ngOnInit() {
    this.processInstanceId = this.activatedRoute.snapshot.queryParamMap.get('processInstanceId');
    this.projectName = this.activatedRoute.snapshot.queryParamMap.get('projectName');
    this.jwt = this.activatedRoute.snapshot.queryParamMap.get('jwt');
    if (this.processInstanceId) {
      this.storageService.set('processInstanceId', this.processInstanceId);
      this.storageService.set('projectName', this.projectName);
      this.storageService.set('jwt', this.jwt);
    }
    console.log('this.processInstanceId - ', this.processInstanceId);
    this.getCurrentStatus();
  }

  onProceedClick() {
    this.route.navigateByUrl('loading');

    const body = {  'projectName': this.projectName }
    // TODO: Send more inputs to the "process" endpoint, as required
    
    const params = '?projectName=' + this.projectName + '&jwt=' + this.jwt;
    this.http.post<any>(this.backendUrl + '/process/' + this.processInstanceId + params, body).subscribe(
      (data) => {
        console.log('Finished processing - ', data);
        this.route.navigateByUrl('result');
      },
      (error) => {
        console.error('Error occurred when initating process - ', error);
      }
    );
  }

  getCurrentStatus() {
    const opts = {
      headers: new HttpHeaders({})
    };
    opts['responseType'] = 'text';
    return this.http.get<any>(this.backendUrl + '/status/' + this.processInstanceId, opts).subscribe(
      (status) => {
        if (status === 'success' || status === 'fail') {
          // Navigate to result screen, if processing is completed
          this.route.navigateByUrl('result');
        } else if (status === 'InProgress') {
          // Navigate to loading screen, if processing is in progress
          this.route.navigateByUrl('loading');
        }
      },
      (error) => {
        console.error('Error occurred when fetching status - ', error);
      }
    );
  }

}
