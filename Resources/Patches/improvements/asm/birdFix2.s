.thumb
ldrb	r2,[r4,#0x09]
cmp	r2,#0x6A
bne	checkbird
ldrb	r1,[r4,#0x0A]
cmp	r1,#0x12
bne	checkbird
ldrb	r1,[r4,#0x0B]
cmp	r1,#0x09
bne	checkbird
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
cmp	r1,#0x68
bne	checkbird
ldrb	r1,[r0,#5]
cmp	r1,#0
bne	checkbird
ldr	r1,=#0x800DA5B
ldrb	r1,[r1]
strb	r1,[r4,#0x0B]
ldr	r0,=#0x80A2074
ldr	r2,[r0]
ldr	r1,=#0x800DA5A
ldrb	r1,[r1]
mov	r0,r4
ldr	r3,=#0x80A2065
bx	r3

checkbird:
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
