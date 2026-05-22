// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Linka.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
             new ApiResource[]
             {
                new ApiResource("ResourceCatalog") {Scopes = {"CatalogFullPermission", "CatalogReadPermission"} },
                new ApiResource("ResourceDiscount") {Scopes = {"DiscountFullPermission"} },
                new ApiResource("ResourceOrder") {Scopes = {"OrderFullPermission"} },
                new ApiResource ("ResourceCargo") {Scopes = {"CargoFullPermission"} },
                new ApiResource ("ResourceBasket") {Scopes = {"BasketFullPermission"} },
                new ApiResource ("ResourcePayment") {Scopes = {"PaymentFullPermission"} },
                new ApiResource ("ResourceImages") {Scopes = {"ImagesFullPermission"} },
                new ApiResource ("ResourceComment") {Scopes = {"CommentFullPermission"} },
                new ApiResource ("ResourceOcelot") {Scopes = {"OcelotFullPermission"} },

                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)

             };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() 
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("CatalogFullPermission","Full access to Catalog operations"),
                new ApiScope("CatalogReadPermission","Read access to Catalog operations"),
                new ApiScope("DiscountFullPermission","Full access to Discount operations"),
                new ApiScope("OrderFullPermission","Full access to Order operations"),
                new ApiScope("CargoFullPermission","Full access to Cargo operations"),
                new ApiScope("BasketFullPermission","Full access to Basket operations"),
                new ApiScope("CommentFullPermission","Full access to Comment operations"),
                new ApiScope("PaymentFullPermission","Full access to Payment operations"),
                new ApiScope("ImagesFullPermission","Full access to Images operations"),
                new ApiScope("OcelotFullPermission","Full access to Ocelot operations"),

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //Visitor
                new Client
                {
                    ClientId = "LinkaVisitorId",
                    ClientName = "Linka Visitor User",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("linkasecret".Sha256()) },
                    AllowedScopes = { "CatalogReadPermission", "CatalogFullPermission", "OcelotFullPermission", "CommentFullPermission", "ImagesFullPermission", "CommentFullPermission",
                    IdentityServerConstants.LocalApi.ScopeName },
                    AllowAccessTokensViaBrowser = true,
                },

                //Manager
                new Client
                {
                    ClientId = "LinkaManagerId",
                    ClientName = "Linka Manager User",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("linkasecret".Sha256()) },
                    AllowedScopes = { "CatalogFullPermission", "BasketFullPermission", "OcelotFullPermission", "CommentFullPermission", "PaymentFullPermission", "ImagesFullPermission", "DiscountFullPermission", "OrderFullPermission",
                    IdentityServerConstants.LocalApi.ScopeName,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile}
                },

                //Admin
                new Client
                {
                    ClientId = "LinkaAdminId",
                    ClientName = "Linka Admin User",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("linkasecret".Sha256()) },
                    AllowedScopes = { "CatalogFullPermission", "CatalogReadPermission", "DiscountFullPermission", "OrderFullPermission", "CargoFullPermission", "BasketFullPermission", "OcelotFullPermission", "CommentFullPermission", "PaymentFullPermission", "ImagesFullPermission",
                    IdentityServerConstants.LocalApi.ScopeName,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                    },
                    AccessTokenLifetime= 600
                 }
             };
    }
}