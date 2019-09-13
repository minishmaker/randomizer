.thumb
ldr	r0,=#0x3001002
mov	r1,#4
strb	r1,[r0]
mov	r1,#0
strb	r1,[r0,#1]
strb	r1,[r0,#2]
mov	r0,#0x51
ldr	r3,=#0x807C728
mov	lr,r3
.short	0xF800
ldr	r0,[r4,#0x64]
ldr	r0,[r0,#8]
str	r5,[r0,#0x64]
ldr	r3,=#0x8041D1F
bx	r3
