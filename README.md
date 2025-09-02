# E-Commerce Microservices Platform

A modern, cloud-native e-commerce platform built with .NET microservices architecture. This project demonstrates key microservices patterns and practices using .NET 6+.

## 🏗️ Architecture Overview

### Core Building Blocks
- **BuildingBlocks**: Shared components used across services
  - **Behaviours**: Cross-cutting concerns and pipeline behaviors
  - **CQRS**: Command Query Responsibility Segregation implementation
  - **Exceptions**: Custom exception handling and middleware

### Services

#### Catalog Service (`/src/Services/Catalog.API`)
- Manages product catalog and inventory
- Implements CRUD operations for products
- Handles product categorization and search

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- Docker Desktop (for containerization)
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/ecom-demo-microservices.git
   cd ecom-demo-microservices
   ```

2. **Run with Docker Compose**
   ```bash
   docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
   ```

3. **Access Services**
   - Catalog API: `http://localhost:8000`
   - API Documentation: `http://localhost:8000/swagger`

## 🛠 Development

### Project Structure
```
src/
├── BuildingBlocks/     # Shared components
│   ├── Behaviours/    # MediatR behaviors
│   ├── CQRS/          # CQRS implementation
│   └── Exceptions/    # Custom exceptions
└── Services/
    └── Catalog.API/   # Catalog microservice
        ├── Data/      # Data access
        ├── Models/    # Domain models
        └── Features/  # Feature folders
```

### Adding a New Service
1. Create a new service under `src/Services`
2. Reference necessary BuildingBlocks
3. Configure in `docker-compose.yml`

## 📚 Documentation

- [Architecture Decision Records](./docs/adr/)
- [API Documentation](./docs/api/)
- [Development Guidelines](./docs/development.md)

## 🤝 Contributing

Contributions are welcome! Please read our [contributing guidelines](CONTRIBUTING.md) before submitting pull requests.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

For any questions or feedback, please open an issue or contact the maintainers.
