# SAS API Gateway

This project is the **API Gateway** for the Situational Awareness System (SAS), implemented using **YARP (Yet Another Reverse Proxy)** in .NET. It provides a single entry point for clients to access microservices.

---


## Project Structure

```

/src
├── ApiGateway/                 # YARP gateway project
│   ├── Program.cs              # YARP + JWT auth configuration
│   ├── appsettings.json        # YARP routes & clusters config
│   └── ReverseProxyConfig/     # Optional external route files

````

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/HasanKhadd0ur/sas-gateway.git
cd sas-gateway/src/ApiGateway
````
## Dependencies

* [YARP - Yet Another Reverse Proxy](https://github.com/microsoft/reverse-proxy)

---

## About

This API Gateway is part of the SAS project, which aggregates real-time social media data to detect and visualize local events like crimes and disasters. The gateway centralizes routing and authentication, making the system easier to manage and secure.

---

## License

MIT License © 2025 Hasan Khaddour
