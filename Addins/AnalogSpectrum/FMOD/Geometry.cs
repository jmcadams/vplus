using System;
using System.Runtime.InteropServices;

namespace FMOD
{
    public class Geometry
    {
        private IntPtr geometryraw;

        public RESULT addPolygon(float directOcclusion, float reverbOcclusion, bool doubleSided, int numVertices, ref VECTOR vertices, ref int polygonIndex)
        {
            return FMOD_Geometry_AddPolygon(this.geometryraw, directOcclusion, reverbOcclusion, doubleSided, numVertices, ref vertices, ref polygonIndex);
        }

        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_AddPolygon(IntPtr geometry, float directOcclusion, float reverbOcclusion, bool doubleSided, int numVertices, ref VECTOR vertices, ref int polygonIndex);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_Flush(IntPtr geometry);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetActive(IntPtr gemoetry, ref bool active);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetMaxPolygons(IntPtr geometry, ref int maxPolygons, ref int maxVertices);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetNumPolygons(IntPtr geometry, ref int numPolygons);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetPolygonAttributes(IntPtr geometry, int polygonIndex, ref float directOcclusion, ref float reverbOcclusion, ref bool doubleSided);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetPolygonNumVertices(IntPtr geometry, int polygonIndex, ref int numVertices);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetPolygonVertex(IntPtr geometry, int polygonIndex, int vertexIndex, ref VECTOR vertex);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetPosition(IntPtr geometry, ref VECTOR position);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetRotation(IntPtr geometry, ref VECTOR forward, ref VECTOR up);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetScale(IntPtr geometry, ref VECTOR scale);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_GetUserData(IntPtr geometry, ref IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_Release(IntPtr geometry);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_Save(IntPtr geometry, IntPtr data, ref int datasize);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetActive(IntPtr gemoetry, bool active);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetPolygonAttributes(IntPtr geometry, int polygonIndex, float directOcclusion, float reverbOcclusion, bool doubleSided);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetPolygonVertex(IntPtr geometry, int polygonIndex, int vertexIndex, ref VECTOR vertex);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetPosition(IntPtr geometry, ref VECTOR position);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetRotation(IntPtr geometry, ref VECTOR forward, ref VECTOR up);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetScale(IntPtr geometry, ref VECTOR scale);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Geometry_SetUserData(IntPtr geometry, IntPtr userdata);
        public RESULT getActive(ref bool active)
        {
            return FMOD_Geometry_GetActive(this.geometryraw, ref active);
        }

        public RESULT getMaxPolygons(ref int maxPolygons, ref int maxVertices)
        {
            return FMOD_Geometry_GetMaxPolygons(this.geometryraw, ref maxPolygons, ref maxVertices);
        }

        public RESULT getNumPolygons(ref int numPolygons)
        {
            return FMOD_Geometry_GetNumPolygons(this.geometryraw, ref numPolygons);
        }

        public RESULT getPolygonAttributes(int polygonIndex, ref float directOcclusion, ref float reverbOcclusion, ref bool doubleSided)
        {
            return FMOD_Geometry_GetPolygonAttributes(this.geometryraw, polygonIndex, ref directOcclusion, ref reverbOcclusion, ref doubleSided);
        }

        public RESULT getPolygonNumVertices(int polygonIndex, ref int numVertices)
        {
            return FMOD_Geometry_GetPolygonNumVertices(this.geometryraw, polygonIndex, ref numVertices);
        }

        public RESULT getPolygonVertex(int polygonIndex, int vertexIndex, ref VECTOR vertex)
        {
            return FMOD_Geometry_GetPolygonVertex(this.geometryraw, polygonIndex, vertexIndex, ref vertex);
        }

        public RESULT getPosition(ref VECTOR position)
        {
            return FMOD_Geometry_GetPosition(this.geometryraw, ref position);
        }

        public IntPtr getRaw()
        {
            return this.geometryraw;
        }

        public RESULT getRotation(ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_Geometry_GetRotation(this.geometryraw, ref forward, ref up);
        }

        public RESULT getScale(ref VECTOR scale)
        {
            return FMOD_Geometry_GetScale(this.geometryraw, ref scale);
        }

        public RESULT getUserData(ref IntPtr userdata)
        {
            return FMOD_Geometry_GetUserData(this.geometryraw, ref userdata);
        }

        public RESULT release()
        {
            return FMOD_Geometry_Release(this.geometryraw);
        }

        public RESULT save(IntPtr data, ref int datasize)
        {
            return FMOD_Geometry_Save(this.geometryraw, data, ref datasize);
        }

        public RESULT setActive(bool active)
        {
            return FMOD_Geometry_SetActive(this.geometryraw, active);
        }

        public RESULT setPolygonAttributes(int polygonIndex, float directOcclusion, float reverbOcclusion, bool doubleSided)
        {
            return FMOD_Geometry_SetPolygonAttributes(this.geometryraw, polygonIndex, directOcclusion, reverbOcclusion, doubleSided);
        }

        public RESULT setPolygonVertex(int polygonIndex, int vertexIndex, ref VECTOR vertex)
        {
            return FMOD_Geometry_SetPolygonVertex(this.geometryraw, polygonIndex, vertexIndex, ref vertex);
        }

        public RESULT setPosition(ref VECTOR position)
        {
            return FMOD_Geometry_SetPosition(this.geometryraw, ref position);
        }

        public void setRaw(IntPtr geometry)
        {
            this.geometryraw = new IntPtr();
            this.geometryraw = geometry;
        }

        public RESULT setRotation(ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_Geometry_SetRotation(this.geometryraw, ref forward, ref up);
        }

        public RESULT setScale(ref VECTOR scale)
        {
            return FMOD_Geometry_SetScale(this.geometryraw, ref scale);
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_Geometry_SetUserData(this.geometryraw, userdata);
        }
    }
}

