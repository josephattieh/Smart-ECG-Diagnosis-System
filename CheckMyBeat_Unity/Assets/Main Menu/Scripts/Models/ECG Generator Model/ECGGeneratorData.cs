namespace Models
{
    public class ECGGeneratorData
    {
        public float defaultGeneratorValues;
        public float heartRate;
        public float amp_pwav;
        public float dist_pwav;
        public float time_pwav;
        public float amp_qwav;
        public float dist_qwav;
        public float time_qwav;
        public float amp_qrswav;
        public float dist_qrswav;
        public float amp_swav;
        public float dist_swav;
        public float amp_twav;
        public float dist_twav;
        public float time_twav;
        public float amp_uwav;
        public float dist_uwav;

        public ECGGeneratorData()
        {
            this.defaultGeneratorValues = 1.0f;
        }

        public ECGGeneratorData(float hR, float a_pwv, float d_pwv, float t_pwv, float a_qwv, float d_qwv, float t_qwv, float a_qrs, float d_qrs, float a_swv, float d_swv, float a_twv, float d_twv, float t_twv, float a_uwv, float d_uwv)
        {
            this.heartRate = hR;
            this.amp_pwav = a_pwv;
            this.dist_pwav = d_pwv;
            this.time_pwav = t_pwv;
            this.amp_qwav = a_qwv;
            this.dist_qwav = d_qwv;
            this.time_qwav = t_qwv;
            this.amp_qrswav = a_qrs;
            this.dist_qrswav = d_qrs;
            this.amp_swav = a_swv;
            this.dist_swav = d_swv;
            this.amp_twav = a_twv;
            this.dist_twav = d_twv;
            this.time_twav = t_twv;
            this.amp_uwav = a_uwv;
            this.dist_uwav = d_uwv;
        }
    }
}
