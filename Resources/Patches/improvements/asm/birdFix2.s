.thumb
ldrb	r2,[r4,#0x09]
cmp	r2,#0x95
bne	vanilla
ldrb	r1,[r4,#0x0E]
cmp	r1,#0xFE
blo	vanilla
mov	r2,#0
vanilla:
lsl	r2,#3
ldr	r0,=#0x80A2074
ldr	r0,[r0]
add	r2,r0
ldrb	r1,[r4,#0x0A]
mov	r0,r4
ldr	r3,=#0x80A2065
bx	r3
