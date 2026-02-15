using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cortexa.Domain.Common
{
    public abstract class BaseEntity
    {
        public string Id { get; protected set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }

        protected BaseEntity()
        {
            Id = GenerateId();
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Generates an ID with format: {PREFIX}-{GUID}
        /// Prefix is derived from the entity class name (e.g., "Patient" -> "PAT", "Admission" -> "ADM")
        /// </summary>
        protected virtual string GenerateId()
        {
            var entityType = GetType();
            var prefix = GetEntityPrefix(entityType.Name);
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(); // 12-char uppercase GUID segment
            
            return $"{prefix}-{uniqueId}";
        }

        /// <summary>
        /// Generates a prefix from the entity class name
        /// Uses explicit mappings for common entities, falls back to automatic generation
        /// </summary>
        private static string GetEntityPrefix(string className)
        {
            // Explicit mappings for common entities
            var prefixMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Actors
                { "Patient", "PAT" },
                { "Doctor", "DOC" },
                { "Nurse", "NUR" },
                
                // Core
                { "Admission", "ADM" },
                
                // Infrastructure
                { "Room", "ROM" },
                { "Bed", "BED" },
                
                // Clinical
                { "VitalSigns", "VIT" },
                { "NursingNotes", "NOT" },
                { "FluidBalance", "FLU" },
                { "Medications", "MED" },
                { "CaseHistory", "CAS" },
                { "PhysicalExamination", "PHY" },
                { "InterventionProcedure", "INT" },
                { "ClinicalIntervention", "CLI" },
                
                // Diagnostics
                { "LabOrder", "LAB" },
                { "LabResult", "RES" },
                { "Imaging", "IMG" },
                { "Culture", "CUL" },
                
                // AI
                { "Alert", "ALT" },
                { "AlertOverrideLog", "AOL" },
                { "RAGQuery", "RAG" },
                { "KnowledgeSource", "KNO" }
            };

            // Check explicit mapping first
            if (prefixMap.TryGetValue(className, out var mappedPrefix))
            {
                return mappedPrefix;
            }

            // Fallback: Remove common suffixes and generate prefix
            var name = className
                .Replace("Entity", "")
                .Replace("Model", "")
                .Trim();

            // For short names, use as-is
            if (name.Length <= 3)
            {
                return name.ToUpper();
            }

            // Extract uppercase letters for acronym
            var prefix = new StringBuilder();
            foreach (var c in name)
            {
                if (char.IsUpper(c) && prefix.Length < 3)
                {
                    prefix.Append(c);
                }
            }

            // If we got 3 uppercase letters, use them
            if (prefix.Length == 3)
            {
                return prefix.ToString();
            }

            // Otherwise, take first 3 characters
            return name.Substring(0, Math.Min(3, name.Length)).ToUpper();
        }
    }
}
