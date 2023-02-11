.thumb
ldrh	r3, [r4, #0x14]
lsr	r3, #1
lsl	r3, #1
strh	r3, [r4, #0x14]
ldr	r3, =#0x8078C48
mov	lr, r3
.short	0xF800
mov	r1, #0x2E
ldsh	r0, [r4, r1]
ldr	r3, =#0x8072560
mov	lr, r3
.short	0xF800