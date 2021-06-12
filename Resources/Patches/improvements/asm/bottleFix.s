.thumb
strh	r0, [r4, #0x12]
ldrh	r0, [r4, #0x10]
mov	r3, #0x80
orr	r0, r3
strh	r0, [r4, #0x10]
mov	r0, #0xFF
and	r2, r0
mov	r0, r4
mov	r1, r2
ldr	r3, =#0x8078A8A
mov	lr,r3
.short	0xF800
