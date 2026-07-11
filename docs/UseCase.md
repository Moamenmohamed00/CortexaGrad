# Cortexia Use Case Diagrams

This document contains Mermaid use-case-style diagrams for Cortexia.

Mermaid does not provide a native UML use-case diagram syntax, so these diagrams use `flowchart` with:

- Actors outside the system boundary.
- Use cases inside grouped subgraphs.
- Directed links from actors to the use cases they initiate.

## 1. Full System Use Case Diagram

```mermaid
flowchart LR
  Doctor[Doctor]
  Nurse[Nurse]
  Admin[Admin or Staff Manager]
  Client[Frontend Client]
  AIService[External AI/RAG Service]
  EmailProvider[Email Provider]

  subgraph Cortexia[Cortexia Backend]
    subgraph Auth[Authentication]
      Register[Register user]
      Login[Login]
      ForgotPassword[Request password reset]
      ResetPassword[Reset password]
    end

    subgraph PatientCare[Patient and Admission Care]
      CreatePatient[Create patient]
      UpdatePatient[Update patient]
      ViewPatients[View patients]
      AdmitPatient[Admit patient]
      ViewAdmissions[View admissions]
      DischargeAdmission[Discharge admission]
      AssignBed[Assign room and bed]
    end

    subgraph Clinical[Clinical Documentation]
      RecordVitals[Record vital signs]
      ReviewVitals[Review vitals history]
      AddCaseHistory[Add case history]
      AddPhysicalExam[Add physical examination]
      AddNursingNote[Add nursing note]
      PrescribeMedication[Prescribe medication]
      RecordFluidBalance[Record fluid balance]
      AddIntervention[Add intervention procedure]
    end

    subgraph Diagnostics[Diagnostics]
      CreateLabOrder[Create lab order]
      AddLabResult[Add lab result]
      UploadImaging[Upload imaging]
      ViewDiagnostics[View diagnostics]
    end

    subgraph SmartAssistant[Smart Assistant and Alerts]
      AskRAG[Ask RAG question]
      UploadKnowledge[Upload knowledge source]
      ViewRAGHistory[View patient RAG history]
      ViewActiveAlerts[View active alerts]
      OverrideAlert[Override alert]
      ReceiveRealtimeAlert[Receive realtime alert]
    end

    subgraph StaffRooms[Staff and Rooms]
      ViewDoctors[View doctors]
      ViewOnDutyNurses[View on-duty nurses]
      ViewRooms[View rooms]
    end
  end

  Doctor --> Register
  Nurse --> Register
  Doctor --> Login
  Nurse --> Login
  Doctor --> ForgotPassword
  Nurse --> ForgotPassword
  Doctor --> ResetPassword
  Nurse --> ResetPassword

  Doctor --> CreatePatient
  Nurse --> CreatePatient
  Doctor --> UpdatePatient
  Nurse --> UpdatePatient
  Doctor --> ViewPatients
  Nurse --> ViewPatients
  Doctor --> AdmitPatient
  Nurse --> AdmitPatient
  Doctor --> ViewAdmissions
  Nurse --> ViewAdmissions
  Doctor --> DischargeAdmission
  Nurse --> DischargeAdmission
  Doctor --> AssignBed
  Nurse --> AssignBed

  Nurse --> RecordVitals
  Doctor --> ReviewVitals
  Nurse --> ReviewVitals
  Doctor --> AddCaseHistory
  Doctor --> AddPhysicalExam
  Nurse --> AddNursingNote
  Doctor --> PrescribeMedication
  Nurse --> RecordFluidBalance
  Nurse --> AddIntervention

  Doctor --> CreateLabOrder
  Nurse --> AddLabResult
  Doctor --> UploadImaging
  Doctor --> ViewDiagnostics
  Nurse --> ViewDiagnostics

  Doctor --> AskRAG
  Doctor --> UploadKnowledge
  Doctor --> ViewRAGHistory
  Doctor --> ViewActiveAlerts
  Nurse --> ViewActiveAlerts
  Doctor --> OverrideAlert
  Doctor --> ReceiveRealtimeAlert
  Nurse --> ReceiveRealtimeAlert

  Doctor --> ViewDoctors
  Nurse --> ViewDoctors
  Doctor --> ViewOnDutyNurses
  Nurse --> ViewOnDutyNurses
  Doctor --> ViewRooms
  Nurse --> ViewRooms

  Client --> Login
  Client --> ReceiveRealtimeAlert
  AskRAG --> AIService
  UploadKnowledge --> AIService
  ForgotPassword --> EmailProvider
```

## 2. Doctor Use Cases

```mermaid
flowchart LR
  Doctor[Doctor]

  subgraph Cortexia[Cortexia Backend]
    Login[Login]
    ManagePatients[Create or update patients]
    ManageAdmissions[Admit, view, or discharge patients]
    ReviewClinicalData[Review clinical data]
    WriteDoctorNotes[Add case history and physical exam]
    PrescribeMedication[Prescribe medication]
    OrderDiagnostics[Order labs and imaging]
    UseAssistant[Ask Smart Assistant]
    ManageAlerts[View or override alerts]
    ViewRoomsAndStaff[View rooms and staff]
  end

  Doctor --> Login
  Doctor --> ManagePatients
  Doctor --> ManageAdmissions
  Doctor --> ReviewClinicalData
  Doctor --> WriteDoctorNotes
  Doctor --> PrescribeMedication
  Doctor --> OrderDiagnostics
  Doctor --> UseAssistant
  Doctor --> ManageAlerts
  Doctor --> ViewRoomsAndStaff
```

## 3. Nurse Use Cases

```mermaid
flowchart LR
  Nurse[Nurse]

  subgraph Cortexia[Cortexia Backend]
    Login[Login]
    ViewPatients[View patients]
    AssistAdmissions[Assist admission workflows]
    RecordVitals[Record vital signs]
    WriteNursingNotes[Write nursing notes]
    RecordFluidBalance[Record fluid balance]
    AddIntervention[Add intervention procedure]
    EnterLabResults[Enter lab results]
    ViewAlerts[View active and realtime alerts]
    ViewRoomsAndStaff[View rooms and staff]
  end

  Nurse --> Login
  Nurse --> ViewPatients
  Nurse --> AssistAdmissions
  Nurse --> RecordVitals
  Nurse --> WriteNursingNotes
  Nurse --> RecordFluidBalance
  Nurse --> AddIntervention
  Nurse --> EnterLabResults
  Nurse --> ViewAlerts
  Nurse --> ViewRoomsAndStaff
```

## 4. Smart Assistant Use Cases

```mermaid
flowchart LR
  Doctor[Doctor]
  Nurse[Nurse]
  AIService[External AI/RAG Service]
  SignalRClient[Connected Client]

  subgraph Cortexia[Cortexia Smart Assistant]
    AskQuestion[Ask clinical RAG question]
    UploadKnowledge[Upload knowledge source]
    ViewPatientHistory[View patient RAG history]
    GetRAGInfo[View RAG service info]
    ViewAlerts[View active alerts]
    OverrideAlert[Override alert]
    BroadcastAlert[Broadcast realtime alert]
  end

  Doctor --> AskQuestion
  Doctor --> UploadKnowledge
  Doctor --> ViewPatientHistory
  Doctor --> GetRAGInfo
  Doctor --> ViewAlerts
  Nurse --> ViewAlerts
  Doctor --> OverrideAlert

  AskQuestion --> AIService
  UploadKnowledge --> AIService
  BroadcastAlert --> SignalRClient
```

## 5. Authentication and Recovery Use Cases

```mermaid
flowchart LR
  User[Doctor or Nurse]
  EmailProvider[Email Provider]

  subgraph Cortexia[Cortexia Auth Module]
    Register[Register]
    Login[Login]
    ForgotPassword[Forgot password]
    ResetPassword[Reset password]
    IssueJwt[Issue JWT]
    ValidateToken[Validate bearer token]
  end

  User --> Register
  User --> Login
  Login --> IssueJwt
  User --> ForgotPassword
  ForgotPassword --> EmailProvider
  User --> ResetPassword
  User --> ValidateToken
```
